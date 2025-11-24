namespace SupermarketApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImagePath { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
