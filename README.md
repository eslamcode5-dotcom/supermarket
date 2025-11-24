# Supermarket Management System

A comprehensive Windows desktop application for managing supermarket operations.

## Features

### 1. Product Catalogue
- Add, update, and view products
- Product images support
- Product descriptions and pricing
- Category management
- Barcode support

### 2. Stock Management
- Real-time stock tracking
- Barcode scanning for quick product lookup
- Add/remove stock with quantity multiplier
- Visual stock levels in grid view

### 3. Point of Sale (POS)
- Fast barcode scanning
- Shopping cart management
- Real-time total calculation
- Stock validation before checkout
- Order completion and processing

### 4. Order Management
- View order history
- Track order status
- Order details with items

### 5. CRM (Customer Relationship Management)
- Customer database
- Purchase history tracking
- Customer information management

## Technology Stack

- .NET 8.0 Windows Forms
- SQLite Database
- C# 12

## Getting Started

### For End Users (Running the Application)

**No installation required!** Just double-click `SupermarketApp.exe` and start using it.

See `QUICK_START.md` for detailed usage instructions.

### For Developers (Building from Source)

**Prerequisites:**
- .NET 8.0 SDK
- Windows OS

**Building the Standalone EXE:**

1. Run `BUILD.bat` (easiest method)
   OR
2. Use command line:
   ```cmd
   dotnet publish SupermarketApp/SupermarketApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./Release
   ```

The standalone executable will be created in the `Release` folder.

See `BUILD_INSTRUCTIONS.md` for detailed build instructions.

### First Run

The application will automatically create the SQLite database on first run.

## Usage

### Adding Products
1. Go to Products > Product Catalogue
2. Fill in product details (name, description, barcode, price, stock, category)
3. Optionally add a product image
4. Click "Add Product"

### Managing Stock
1. Go to Products > Stock Management
2. Scan or enter product barcode
3. Enter quantity
4. Click "Add Stock" or "Remove Stock"

### Processing Sales
1. Go to Sales > Point of Sale (POS)
2. Scan product barcodes
3. Adjust quantities as needed
4. Review cart
5. Click "Checkout" to complete the sale

### Viewing Orders
1. Go to Sales > Order Management
2. View all completed orders with details

## Database Schema

- **Products**: Product information and inventory
- **Customers**: Customer details and purchase history
- **Orders**: Order headers
- **OrderItems**: Individual items in each order

## Future Enhancements

- Receipt printing
- Advanced reporting and analytics
- Barcode label printing
- Multi-user support with authentication
- Supplier management
- Purchase orders
- Returns and refunds processing
