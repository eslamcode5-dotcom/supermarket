using System.Data.SQLite;
using SupermarketApp.Data;
using SupermarketApp.Models;

namespace SupermarketApp.Services
{
    public class OrderService
    {
        private readonly DatabaseManager dbManager;
        private readonly ProductService productService;

        public OrderService(DatabaseManager dbManager, ProductService productService)
        {
            this.dbManager = dbManager;
            this.productService = productService;
        }

        public int CreateOrder(Order order)
        {
            using var connection = dbManager.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                string orderQuery = @"INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, Status)
                                    VALUES (@CustomerId, @OrderDate, @TotalAmount, @Status);
                                    SELECT last_insert_rowid();";

                using var cmd = new SQLiteCommand(orderQuery, connection);
                cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate.ToString("o"));
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@Status", order.Status);

                int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var item in order.Items)
                {
                    string itemQuery = @"INSERT INTO OrderItems (OrderId, ProductId, ProductName, Quantity, UnitPrice)
                                       VALUES (@OrderId, @ProductId, @ProductName, @Quantity, @UnitPrice)";

                    using var itemCmd = new SQLiteCommand(itemQuery, connection);
                    itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                    itemCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                    itemCmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    itemCmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                    itemCmd.ExecuteNonQuery();

                    productService.UpdateStock(item.ProductId, -item.Quantity);
                }

                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Orders ORDER BY OrderDate DESC";
            using var cmd = new SQLiteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var order = new Order
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    CustomerId = Convert.ToInt32(reader["CustomerId"]),
                    OrderDate = DateTime.Parse(reader["OrderDate"].ToString() ?? DateTime.Now.ToString()),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    Status = reader["Status"].ToString() ?? "Pending"
                };
                orders.Add(order);
            }

            return orders;
        }
    }
}
