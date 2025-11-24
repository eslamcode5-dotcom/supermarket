using SupermarketApp.Services;

namespace SupermarketApp.Forms
{
    public partial class OrderManagementForm : Form
    {
        private readonly OrderService orderService;
        private DataGridView dgvOrders;

        public OrderManagementForm(OrderService orderService)
        {
            this.orderService = orderService;
            InitializeComponent();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.Text = "Order Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10) };
            
            var btnRefresh = new Button { Text = "Refresh", Location = new Point(10, 15), Width = 100 };
            btnRefresh.Click += (s, e) => LoadOrders();
            topPanel.Controls.Add(btnRefresh);

            var lblTitle = new Label 
            { 
                Text = "Order History", 
                Location = new Point(120, 18), 
                Width = 200,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };
            topPanel.Controls.Add(lblTitle);

            this.Controls.Add(topPanel);

            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };

            this.Controls.Add(dgvOrders);
        }

        private void LoadOrders()
        {
            var orders = orderService.GetAllOrders();
            dgvOrders.DataSource = orders;
        }
    }
}
