using SupermarketApp.Models;
using SupermarketApp.Services;

namespace SupermarketApp.Forms
{
    public partial class StockManagementForm : Form
    {
        private readonly ProductService productService;
        private DataGridView dgvStock;
        private TextBox txtBarcodeScan, txtQuantity;
        private Button btnScan, btnAddStock, btnRemoveStock;
        private Label lblScannedProduct;

        public StockManagementForm(ProductService productService)
        {
            this.productService = productService;
            InitializeComponent();
            LoadStock();
        }

        private void InitializeComponent()
        {
            this.Text = "Stock Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 120, Padding = new Padding(10) };

            topPanel.Controls.Add(new Label { Text = "Scan Barcode:", Location = new Point(10, 15), Width = 100 });
            txtBarcodeScan = new TextBox { Location = new Point(120, 12), Width = 200 };
            txtBarcodeScan.KeyPress += TxtBarcodeScan_KeyPress;
            topPanel.Controls.Add(txtBarcodeScan);

            btnScan = new Button { Text = "Scan", Location = new Point(330, 10), Width = 80 };
            btnScan.Click += BtnScan_Click;
            topPanel.Controls.Add(btnScan);

            lblScannedProduct = new Label 
            { 
                Location = new Point(10, 45), 
                Width = 400, 
                Height = 20,
                ForeColor = Color.Blue,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            topPanel.Controls.Add(lblScannedProduct);

            topPanel.Controls.Add(new Label { Text = "Quantity:", Location = new Point(10, 75), Width = 100 });
            txtQuantity = new TextBox { Location = new Point(120, 72), Width = 100 };
            topPanel.Controls.Add(txtQuantity);

            btnAddStock = new Button { Text = "Add Stock", Location = new Point(230, 70), Width = 90, BackColor = Color.LightGreen };
            btnAddStock.Click += BtnAddStock_Click;
            topPanel.Controls.Add(btnAddStock);

            btnRemoveStock = new Button { Text = "Remove Stock", Location = new Point(330, 70), Width = 100, BackColor = Color.LightCoral };
            btnRemoveStock.Click += BtnRemoveStock_Click;
            topPanel.Controls.Add(btnRemoveStock);

            this.Controls.Add(topPanel);

            dgvStock = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };

            this.Controls.Add(dgvStock);
        }

        private void LoadStock()
        {
            var products = productService.GetAllProducts();
            dgvStock.DataSource = products;
            
            if (dgvStock.Columns.Contains("ImagePath"))
                dgvStock.Columns["ImagePath"].Visible = false;
        }

        private void TxtBarcodeScan_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnScan_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnScan_Click(object? sender, EventArgs e)
        {
            string barcode = txtBarcodeScan.Text.Trim();
            if (string.IsNullOrEmpty(barcode)) return;

            var product = productService.GetProductByBarcode(barcode);
            if (product != null)
            {
                lblScannedProduct.Text = $"Product: {product.Name} | Current Stock: {product.StockQuantity} | Price: ${product.Price}";
                txtQuantity.Focus();
            }
            else
            {
                lblScannedProduct.Text = "Product not found!";
                lblScannedProduct.ForeColor = Color.Red;
            }
        }

        private void BtnAddStock_Click(object? sender, EventArgs e)
        {
            UpdateStock(1);
        }

        private void BtnRemoveStock_Click(object? sender, EventArgs e)
        {
            UpdateStock(-1);
        }

        private void UpdateStock(int multiplier)
        {
            string barcode = txtBarcodeScan.Text.Trim();
            if (string.IsNullOrEmpty(barcode))
            {
                MessageBox.Show("Please scan a product first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = productService.GetProductByBarcode(barcode);
            if (product != null)
            {
                productService.UpdateStock(product.Id, quantity * multiplier);
                MessageBox.Show($"Stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStock();
                txtBarcodeScan.Clear();
                txtQuantity.Clear();
                lblScannedProduct.Text = "";
                txtBarcodeScan.Focus();
            }
        }
    }
}
