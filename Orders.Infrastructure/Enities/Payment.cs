using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Enities
{
    [Table("PAYMENT")]  
    public class PAYMENT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ORDERID { get; set; }

        [StringLength(50)]
        public string PAYMENTMETHOD { get; set; }

        public decimal AMOUNT { get; set; }

        [StringLength(20)]
        public string ADDRESS { get; set; }

        public DateTime? DATEPAYMENT { get; set; }
    }
}
