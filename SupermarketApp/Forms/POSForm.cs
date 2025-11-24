using SupermarketApp.Models;
using SupermarketApp.Services;

namespace SupermarketApp.Forms
{
    public partial class POSForm : Form
    {
        private readonly ProductService productService;
        private readonly OrderService orderService;
        private List<OrderItem> cartItems = new();
        private DataGridView dgvCart;
        private TextBox txtBarcode, txtQuantity;
        private Label lblTotal, lblProductInfo;
        private Button btnAddToCart, btnRemoveItem, btnCheckout, btnClear;

        public POSForm(ProductService productService, OrderService orderService)
        {
            this.productService = productService;
            this.orderService = orderService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Point of Sale (POS)";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 150, Padding = new Padding(10) };

            topPanel.Controls.Add(new Label { Text = "Scan/Enter Barcode:", Location = new Point(10, 15), Width = 120 });
            txtBarcode = new TextBox { Location = new Point(140, 12), Width = 250 };
            txtBarcode.KeyPress += TxtBarcode_KeyPress;
            topPanel.Controls.Add(txtBarcode);

            topPanel.Controls.Add(new Label { Text = "Quantity:", Location = new Point(410, 15), Width = 70 });
            txtQuantity = new TextBox { Location = new Point(490, 12), Width = 80, Text = "1" };
            topPanel.Controls.Add(txtQuantity);

            btnAddToCart = new Button { Text = "Add to Cart", Location = new Point(580, 10), Width = 100, BackColor = Color.LightGreen };
            btnAddToCart.Click += BtnAddToCart_Click;
            topPanel.Controls.Add(btnAddToCart);

            lblProductInfo = new Label 
            { 
                Location = new Point(10, 50), 
                Width = 670, 
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            topPanel.Controls.Add(lblProductInfo);

            lblTotal = new Label 
            { 
                Text = "Total: $0.00", 
                Location = new Point(10, 100), 
                Width = 300, 
                Height = 40,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Green
            };
            topPanel.Controls.Add(lblTotal);

            btnRemoveItem = new Button { Text = "Remove Item", Location = new Point(320, 105), Width = 100 };
            btnRemoveItem.Click += BtnRemoveItem_Click;
            topPanel.Controls.Add(btnRemoveItem);

            btnClear = new Button { Text = "Clear Cart", Location = new Point(430, 105), Width = 100, BackColor = Color.Orange };
            btnClear.Click += BtnClear_Click;
            topPanel.Controls.Add(btnClear);

            btnCheckout = new Button { Text = "Checkout", Location = new Point(540, 105), Width = 140, Height = 35, BackColor = Color.LightBlue, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            btnCheckout.Click += BtnCheckout_Click;
            topPanel.Controls.Add(btnCheckout);

            this.Controls.Add(topPanel);

            dgvCart = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };

            this.Controls.Add(dgvCart);
            UpdateCart();
        }

        private void TxtBarcode_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnAddToCart_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(barcode)) return;

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = productService.GetProductByBarcode(barcode);
            if (product != null)
            {
                if (product.StockQuantity < quantity)
                {
                    MessageBox.Show($"Insufficient stock! Available: {product.StockQuantity}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existingItem = cartItems.FirstOrDefault(i => i.ProductId == product.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cartItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    });
                }

                lblProductInfo.Text = $"Added: {product.Name} x {quantity} @ ${product.Price} each";
                lblProductInfo.ForeColor = Color.Green;
                UpdateCart();
                txtBarcode.Clear();
                txtQuantity.Text = "1";
                txtBarcode.Focus();
            }
            else
            {
                lblProductInfo.Text = "Product not found!";
                lblProductInfo.ForeColor = Color.Red;
            }
        }

        private void BtnRemoveItem_Click(object? sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count > 0)
            {
                var item = (OrderItem)dgvCart.SelectedRows[0].DataBoundItem;
                cartItems.Remove(item);
                UpdateCart();
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            cartItems.Clear();
            UpdateCart();
            lblProductInfo.Text = "";
        }

        private void BtnCheckout_Click(object? sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var order = new Order
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(i => i.Subtotal),
                Status = "Completed",
                Items = new List<OrderItem>(cartItems)
            };

            try
            {
                int orderId = orderService.CreateOrder(order);
                MessageBox.Show($"Order #{orderId} completed successfully!\nTotal: ${order.TotalAmount:F2}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cartItems.Clear();
                UpdateCart();
                lblProductInfo.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCart()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = cartItems.ToList();
            
            decimal total = cartItems.Sum(i => i.Subtotal);
            lblTotal.Text = $"Total: ${total:F2}";
        }
    }
}
