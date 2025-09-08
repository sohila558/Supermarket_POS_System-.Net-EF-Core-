namespace MySuperMarket.DTOs
{
    public class AddInvoiceDTO
    {
        public required int CustomerId { get; set; }
        public required DateTime InvoiceDate { get; set; }
        public List<InvoiceItemDTO> InvoiceItems { get; set; }
    }
}
