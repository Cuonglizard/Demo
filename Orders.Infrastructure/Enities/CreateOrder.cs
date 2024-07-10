using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Enities
{
    public class PRODUCTS
    {
        public string ORDERID { get; set; }
        public string EMAIL { get; set; }
        public string LOG { get; set; }
        public float? ACCOUNT { get; set; }
        public DateTime? DATECREATE { get; set; }
    }
}

