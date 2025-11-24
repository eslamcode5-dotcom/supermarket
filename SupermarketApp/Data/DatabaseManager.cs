using System.Data.SQLite;
using SupermarketApp.Models;

namespace SupermarketApp.Data
{
    public class DatabaseManager
    {
        private readonly string connectionString;

        public DatabaseManager()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "supermarket.db");
            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string createProductsTable = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Barcode TEXT UNIQUE,
                    Price REAL NOT NULL,
                    StockQuantity INTEGER NOT NULL,
                    ImagePath TEXT,
                    Category TEXT,
                    CreatedDate TEXT,
                    LastModified TEXT
                )";

            string createCustomersTable = @"
                CREATE TABLE IF NOT EXISTS Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT,
                    Phone TEXT,
                    Address TEXT,
                    RegisteredDate TEXT,
                    TotalPurchases REAL
                )";

            string createOrdersTable = @"
                CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER,
                    OrderDate TEXT,
                    TotalAmount REAL,
                    Status TEXT,
                    FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
                )";

            string createOrderItemsTable = @"
                CREATE TABLE IF NOT EXISTS OrderItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderId INTEGER,
                    ProductId INTEGER,
                    ProductName TEXT,
                    Quantity INTEGER,
                    UnitPrice REAL,
                    FOREIGN KEY(OrderId) REFERENCES Orders(Id),
                    FOREIGN KEY(ProductId) REFERENCES Products(Id)
                )";

            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = createProductsTable;
            cmd.ExecuteNonQuery();
            cmd.CommandText = createCustomersTable;
            cmd.ExecuteNonQuery();
            cmd.CommandText = createOrdersTable;
            cmd.ExecuteNonQuery();
            cmd.CommandText = createOrderItemsTable;
            cmd.ExecuteNonQuery();
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
