using System.ComponentModel.DataAnnotations;

namespace MySuperMarket.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required double TotalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public required double UnitPrice { get; set; }
        public Invoice Invoices { get; set; }
        public Product Products { get; set; }
    }
}
