namespace MySuperMarket.DTOs
{
    public class InvoiceItemDTO
    {
        public required int InvoiceId { get; set; }
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required double TotalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public required double UnitPrice { get; set; }
    }
}
