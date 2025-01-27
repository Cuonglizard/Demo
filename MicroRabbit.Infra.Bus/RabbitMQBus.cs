﻿using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Configs _configs;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dbConfigFile = Path.Combine(baseDir, "RabbitMQConnection.json");
            if (File.Exists(dbConfigFile))
            {
                _configs = JsonConvert.DeserializeObject<Configs>(File.ReadAllText(dbConfigFile));
            }
            string exchangeType = "direct";
            var factory = new ConnectionFactory() {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                Port = 5672,
                HostName = "localhost",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30),
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.ExchangeDeclare(eventName, exchangeType);


                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", eventName, null, body);
            }

        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dbConfigFile = Path.Combine(baseDir, "RabbitMQConnection.json");
            if (File.Exists(dbConfigFile))
            {
                _configs = JsonConvert.DeserializeObject<Configs>(File.ReadAllText(dbConfigFile));
            }
            string exchangeType = "direct";
            var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                Port = 5672,
                HostName = "localhost",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30),
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            channel.ExchangeDeclare(eventName, exchangeType);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
    }
}