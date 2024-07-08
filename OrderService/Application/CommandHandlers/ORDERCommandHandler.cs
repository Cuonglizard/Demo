using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MediatR;
using Orders.Application.Commands;
using Orders.Application.Events;

namespace Ordering.Domain.CommandHandlers
{   
    public class ORDERCommandHandler : IRequestHandler<ORDERCommand, bool>
    {
        private readonly IEventBus _eventBus;
        public ORDERCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public Task<bool> Handle(ORDERCommand request, CancellationToken cancellationToken)
        {
            //publish event to rabbitMQ
            _eventBus.Publish(new ORDEREvent(request.CreateOrder));
            return Task.FromResult(true);
        }
    }
}
