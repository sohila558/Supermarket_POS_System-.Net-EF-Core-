namespace MySuperMarket.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerNumber { get; set; }
        public Invoice Invoices { get; set; }
    }
}
