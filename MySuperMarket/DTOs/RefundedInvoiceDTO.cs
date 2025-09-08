namespace MySuperMarket.DTOs
{
    public class RefundedInvoiceDTO
    {
        public List<RefundedInvoiceItemsDTO> items { get; set; }
        public int InvoiceId { get; set; }
    }
}
