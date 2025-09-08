namespace MySuperMarket.DTOs
{
    public class InvoiceDTO
    {
        public required int CustomerId { get; set; }
        public required DateTime InvoiceDate { get; set; }
    }
}
