using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Models
{
    public class CreateOrder
    {
        public int IdUser { get; set; }
        public long OrderId { get; set; }
        public string Email { get; set; }
        public string Log { get; set; }
        public long Account { get; set; }
        public DateTime DateCreate { get; set; }

    }
}