namespace MySuperMarket.DTOs
{
    public class CustomerDTO : IPUT
    {
        public required string CustomerName { get; set; }
        public required string CustomerNumber { get; set; }
        public int Id { get; set; }
    }
}
