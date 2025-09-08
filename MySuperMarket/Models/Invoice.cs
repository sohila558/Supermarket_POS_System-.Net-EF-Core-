using System.ComponentModel.DataAnnotations;

namespace MySuperMarket.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public required DateTime InvoiceDate { get; set; }
        public bool IsRefunded { get; set; }
        public Customer Customers { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
