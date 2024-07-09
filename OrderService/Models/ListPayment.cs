namespace Orders.Models
{
    public class ListPayment
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DatePayment { get; set; }
        public string ProductName { get; set; }
    }
}
