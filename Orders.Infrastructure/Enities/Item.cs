using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Enities
{
    public class ITEM
    {
        public int ID { get; set; }
        public string IMGLINK { get; set; }
        public decimal PRICE { get; set; }
        public string DETAIL { get; set; }
        public int ISBUY { get; set; }
        public DateTime DATEBUY { get; set; }
        public string NAME { get; set; }

    }
}
