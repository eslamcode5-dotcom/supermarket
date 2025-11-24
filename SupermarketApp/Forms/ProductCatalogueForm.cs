using SupermarketApp.Models;
using SupermarketApp.Services;

namespace SupermarketApp.Forms
{
    public partial class ProductCatalogueForm : Form
    {
        private readonly ProductService productService;
        private DataGridView dgvProducts;
        private PictureBox pbProductImage;
        private TextBox txtName, txtDescription, txtBarcode, txtPrice, txtStock, txtCategory;
        private Button btnAdd, btnUpdate, btnDelete, btnBrowseImage;
        private string? selectedImagePath;

        public ProductCatalogueForm(ProductService productService)
        {
            this.productService = productService;
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.Text = "Product Catalogue";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350
            };

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;

            splitContainer.Panel1.Controls.Add(dgvProducts);

            var detailsPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            
            int y = 10;
            detailsPanel.Controls.Add(new Label { Text = "Name:", Location = new Point(10, y), Width = 100 });
            txtName = new TextBox { Location = new Point(120, y), Width = 200 };
            detailsPanel.Controls.Add(txtName);

            y += 30;
            detailsPanel.Controls.Add(new Label { Text = "Description:", Location = new Point(10, y), Width = 100 });
            txtDescription = new TextBox { Location = new Point(120, y), Width = 200, Height = 60, Multiline = true };
            detailsPanel.Controls.Add(txtDescription);

            y += 70;
            detailsPanel.Controls.Add(new Label { Text = "Barcode:", Location = new Point(10, y), Width = 100 });
            txtBarcode = new TextBox { Location = new Point(120, y), Width = 200 };
            detailsPanel.Controls.Add(txtBarcode);

            y += 30;
            detailsPanel.Controls.Add(new Label { Text = "Price:", Location = new Point(10, y), Width = 100 });
            txtPrice = new TextBox { Location = new Point(120, y), Width = 200 };
            detailsPanel.Controls.Add(txtPrice);

            detailsPanel.Controls.Add(new Label { Text = "Stock:", Location = new Point(350, 10), Width = 100 });
            txtStock = new TextBox { Location = new Point(460, 10), Width = 150 };
            detailsPanel.Controls.Add(txtStock);

            detailsPanel.Controls.Add(new Label { Text = "Category:", Location = new Point(350, 40), Width = 100 });
            txtCategory = new TextBox { Location = new Point(460, 40), Width = 150 };
            detailsPanel.Controls.Add(txtCategory);

            pbProductImage = new PictureBox
            {
                Location = new Point(650, 10),
                Size = new Size(150, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            detailsPanel.Controls.Add(pbProductImage);

            btnBrowseImage = new Button { Text = "Browse Image", Location = new Point(650, 170), Width = 150 };
            btnBrowseImage.Click += BtnBrowseImage_Click;
            detailsPanel.Controls.Add(btnBrowseImage);

            btnAdd = new Button { Text = "Add Product", Location = new Point(120, y + 40), Width = 100 };
            btnAdd.Click += BtnAdd_Click;
            detailsPanel.Controls.Add(btnAdd);

            btnUpdate = new Button { Text = "Update", Location = new Point(230, y + 40), Width = 100 };
            btnUpdate.Click += BtnUpdate_Click;
            detailsPanel.Controls.Add(btnUpdate);

            btnDelete = new Button { Text = "Delete", Location = new Point(340, y + 40), Width = 100 };
            btnDelete.Click += BtnDelete_Click;
            detailsPanel.Controls.Add(btnDelete);

            splitContainer.Panel2.Controls.Add(detailsPanel);
            this.Controls.Add(splitContainer);
        }

        private void LoadProducts()
        {
            var products = productService.GetAllProducts();
            dgvProducts.DataSource = products;
        }

        private void DgvProducts_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                var product = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
                txtName.Text = product.Name;
                txtDescription.Text = product.Description;
                txtBarcode.Text = product.Barcode;
                txtPrice.Text = product.Price.ToString();
                txtStock.Text = product.StockQuantity.ToString();
                txtCategory.Text = product.Category;
                selectedImagePath = product.ImagePath;

                if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
                {
                    pbProductImage.Image = Image.FromFile(product.ImagePath);
                }
                else
                {
                    pbProductImage.Image = null;
                }
            }
        }

        private void BtnBrowseImage_Click(object? sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select Product Image"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;
                pbProductImage.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                var product = new Product
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Barcode = txtBarcode.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    StockQuantity = int.Parse(txtStock.Text),
                    Category = txtCategory.Text,
                    ImagePath = selectedImagePath
                };

                productService.AddProduct(product);
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                try
                {
                    var product = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
                    product.Name = txtName.Text;
                    product.Description = txtDescription.Text;
                    product.Barcode = txtBarcode.Text;
                    product.Price = decimal.Parse(txtPrice.Text);
                    product.StockQuantity = int.Parse(txtStock.Text);
                    product.Category = txtCategory.Text;
                    product.ImagePath = selectedImagePath;

                    productService.UpdateProduct(product);
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Delete functionality - implement as needed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtBarcode.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            txtCategory.Clear();
            pbProductImage.Image = null;
            selectedImagePath = null;
        }
    }
}
