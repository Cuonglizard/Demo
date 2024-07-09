namespace Orders.Models
{
    public class ListProducts
    {
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
