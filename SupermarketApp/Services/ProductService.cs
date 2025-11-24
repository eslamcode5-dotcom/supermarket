using System.Data.SQLite;
using SupermarketApp.Data;
using SupermarketApp.Models;

namespace SupermarketApp.Services
{
    public class ProductService
    {
        private readonly DatabaseManager dbManager;

        public ProductService(DatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Products ORDER BY Name";
            using var cmd = new SQLiteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(MapProduct(reader));
            }

            return products;
        }

        public Product? GetProductByBarcode(string barcode)
        {
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Products WHERE Barcode = @Barcode";
            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Barcode", barcode);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapProduct(reader);
            }

            return null;
        }

        public void AddProduct(Product product)
        {
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = @"INSERT INTO Products (Name, Description, Barcode, Price, StockQuantity, ImagePath, Category, CreatedDate, LastModified)
                           VALUES (@Name, @Description, @Barcode, @Price, @StockQuantity, @ImagePath, @Category, @CreatedDate, @LastModified)";

            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
            cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("o"));
            cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("o"));

            cmd.ExecuteNonQuery();
        }

        public void UpdateProduct(Product product)
        {
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = @"UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, 
                           Price = @Price, StockQuantity = @StockQuantity, ImagePath = @ImagePath, 
                           Category = @Category, LastModified = @LastModified WHERE Id = @Id";

            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
            cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString("o"));

            cmd.ExecuteNonQuery();
        }

        public void UpdateStock(int productId, int quantity)
        {
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = "UPDATE Products SET StockQuantity = StockQuantity + @Quantity WHERE Id = @Id";
            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", productId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            cmd.ExecuteNonQuery();
        }

        private Product MapProduct(SQLiteDataReader reader)
        {
            return new Product
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? "",
                Description = reader["Description"].ToString() ?? "",
                Barcode = reader["Barcode"].ToString() ?? "",
                Price = Convert.ToDecimal(reader["Price"]),
                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                ImagePath = reader["ImagePath"] as string,
                Category = reader["Category"].ToString() ?? "",
                CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString() ?? DateTime.Now.ToString()),
                LastModified = DateTime.Parse(reader["LastModified"].ToString() ?? DateTime.Now.ToString())
            };
        }
    }
}
