namespace LR12.ApiService
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ComponentId { get; set; }
        public DateTime SaleDate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? CustomerEmail { get; set; }

        public ComputerComponent? Component { get; set; }
    }
}
