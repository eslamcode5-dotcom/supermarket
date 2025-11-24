using System.Data.SQLite;
using SupermarketApp.Data;
using SupermarketApp.Models;

namespace SupermarketApp.Services
{
    public class CustomerService
    {
        private readonly DatabaseManager dbManager;

        public CustomerService(DatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Customers ORDER BY Name";
            using var cmd = new SQLiteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString() ?? "",
                    Email = reader["Email"].ToString() ?? "",
                    Phone = reader["Phone"].ToString() ?? "",
                    Address = reader["Address"].ToString() ?? "",
                    RegisteredDate = DateTime.Parse(reader["RegisteredDate"].ToString() ?? DateTime.Now.ToString()),
                    TotalPurchases = Convert.ToDecimal(reader["TotalPurchases"])
                });
            }

            return customers;
        }

        public void AddCustomer(Customer customer)
        {
            using var connection = dbManager.GetConnection();
            connection.Open();

            string query = @"INSERT INTO Customers (Name, Email, Phone, Address, RegisteredDate, TotalPurchases)
                           VALUES (@Name, @Email, @Phone, @Address, @RegisteredDate, @TotalPurchases)";

            using var cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Email", customer.Email);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.Parameters.AddWithValue("@Address", customer.Address);
            cmd.Parameters.AddWithValue("@RegisteredDate", customer.RegisteredDate.ToString("o"));
            cmd.Parameters.AddWithValue("@TotalPurchases", customer.TotalPurchases);

            cmd.ExecuteNonQuery();
        }
    }
}
