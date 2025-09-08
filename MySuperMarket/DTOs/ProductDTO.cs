namespace MySuperMarket.DTOs
{
    public class ProductDTO:IPUT
    {
        public required string ProductName { get; set; }
        public required double UnitPrice { get; set; }
        public required int Stock { get; set; }
        public required bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
