using SupermarketApp.Data;
using SupermarketApp.Services;

namespace SupermarketApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly DatabaseManager dbManager;
        private readonly ProductService productService;
        private readonly OrderService orderService;

        public MainForm()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            productService = new ProductService(dbManager);
            orderService = new OrderService(dbManager, productService);
        }

        private void InitializeComponent()
        {
            this.Text = "Supermarket Management System";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            var menuStrip = new MenuStrip();
            
            var productsMenu = new ToolStripMenuItem("Products");
            productsMenu.DropDownItems.Add("Product Catalogue", null, (s, e) => OpenProductCatalogue());
            productsMenu.DropDownItems.Add("Stock Management", null, (s, e) => OpenStockManagement());
            
            var salesMenu = new ToolStripMenuItem("Sales");
            salesMenu.DropDownItems.Add("Point of Sale (POS)", null, (s, e) => OpenPOS());
            salesMenu.DropDownItems.Add("Order Management", null, (s, e) => OpenOrderManagement());
            
            var crmMenu = new ToolStripMenuItem("CRM");
            crmMenu.DropDownItems.Add("Customer Management", null, (s, e) => OpenCRM());

            menuStrip.Items.Add(productsMenu);
            menuStrip.Items.Add(salesMenu);
            menuStrip.Items.Add(crmMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            var welcomeLabel = new Label
            {
                Text = "Welcome to Supermarket Management System\n\nSelect an option from the menu above",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(welcomeLabel);
        }

        private void OpenProductCatalogue()
        {
            var form = new ProductCatalogueForm(productService);
            form.ShowDialog();
        }

        private void OpenStockManagement()
        {
            var form = new StockManagementForm(productService);
            form.ShowDialog();
        }

        private void OpenPOS()
        {
            var form = new POSForm(productService, orderService);
            form.ShowDialog();
        }

        private void OpenOrderManagement()
        {
            var form = new OrderManagementForm(orderService);
            form.ShowDialog();
        }

        private void OpenCRM()
        {
            MessageBox.Show("CRM Module - Coming Soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
