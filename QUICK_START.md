# Quick Start Guide - For End Users

## Running the Application

1. **Double-click** `SupermarketApp.exe`
2. The application will start immediately
3. On first run, it will create a database file in the same folder

## Using the Application

### Main Menu
The application has three main sections:
- **Products** - Manage your inventory
- **Sales** - Process sales and view orders
- **CRM** - Customer management (coming soon)

### Adding Products

1. Click **Products → Product Catalogue**
2. Fill in the product details:
   - Name (required)
   - Description
   - Barcode (required for scanning)
   - Price (required)
   - Stock Quantity
   - Category
3. Click **Browse Image** to add a product photo (optional)
4. Click **Add Product**

### Managing Stock

1. Click **Products → Stock Management**
2. Scan or type the product barcode
3. Press Enter or click **Scan**
4. Enter the quantity
5. Click **Add Stock** to increase or **Remove Stock** to decrease

### Processing Sales (POS)

1. Click **Sales → Point of Sale (POS)**
2. Scan or type product barcodes
3. Adjust quantities if needed
4. Click **Add to Cart**
5. Review the cart and total
6. Click **Checkout** to complete the sale

### Viewing Orders

1. Click **Sales → Order Management**
2. View all completed orders
3. Click **Refresh** to update the list

## Tips

- **Barcode Scanning**: If you have a USB barcode scanner, just scan products - they'll be entered automatically
- **Keyboard Shortcuts**: Press Enter after typing a barcode to quickly add items
- **Stock Alerts**: The system will warn you if there's insufficient stock during checkout
- **Images**: Product images are optional but help identify items quickly

## Data Location

All your data is stored in `supermarket.db` in the same folder as the application.

**Important**: 
- Keep backups of this file regularly
- Don't delete this file or you'll lose all your data
- You can copy this file to backup your entire database

## Support

For issues or questions, refer to the README.md file or contact your system administrator.
