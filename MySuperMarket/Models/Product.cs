namespace MySuperMarket.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required double UnitPrice { get; set; }
        public required int Stock { get; set; }
        public required bool IsDeleted { get; set; }
        public InvoiceItem InvoiceItems { get; set; }
    }
}
