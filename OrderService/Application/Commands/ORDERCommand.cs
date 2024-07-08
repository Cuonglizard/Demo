using MicroRabbit.Domain.Core.Commands;
using Orders.Application.Models;

namespace Orders.Application.Commands
{
    public class ORDERCommand : Command
    {
        public CreateOrder CreateOrder { get; set; }
        public ORDERCommand(CreateOrder createOrder)
        {
            CreateOrder = createOrder;
        }
    }
}
