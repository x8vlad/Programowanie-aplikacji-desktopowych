using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ElektroBud
{
    
    public class Product
    {
        public string Ean { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Sector { get; set; } = "";
        public string Rack { get; set; } = "";
        public string Shelf { get; set; } = "";
        public int Quantity { get; set; }
    }

    public partial class MainWindow : Window
    {
        private List<Product> products = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            string searchValue = SearchBox.Text.Trim().ToLower();

            List<Product> foundProduct = new List<Product>();

            foreach (var product in products)
            {
                if (product.Ean.ToLower().Contains(searchValue) || product.Name.ToLower().Contains(searchValue))
                {
                    foundProduct.Add(product);
                }
            }

            ProductsTable.ItemsSource = foundProduct;
        }

        private bool ValidateForm(out int quantity)
        {
            quantity = 0;
            string textQuantity = QuantityBox.Text.Trim();
            bool isQuantityValid = int.TryParse(textQuantity, out quantity);

            if (EanBox.Text.Trim() == "" || NameBox.Text.Trim() == "" || CategoryBox.Text.Trim() == "" || SectorBox.Text.Trim() == "" || RackBox.Text.Trim() == "" || ShelfBox.Text.Trim() == "")
            {
                MessageBox.Show("Wszystkie pola są wymagane");
                return false;
            }
            
            if(quantity < 0)
            {
                MessageBox.Show("Ilość nie może być ujemna.");
                return false;
            }

            if(!isQuantityValid)
            {
                MessageBox.Show("Ilość musi być liczbą całkowitą.");
                return false;
            }

            return true;
        }

        private void FormToProduct(Product p, int quantity)
        {
            p.Ean = EanBox.Text.Trim();
            p.Name = NameBox.Text.Trim();
            p.Category = CategoryBox.Text.Trim();
            p.Sector = SectorBox.Text.Trim();
            p.Rack = RackBox.Text.Trim();
            p.Shelf = ShelfBox.Text.Trim();
            p.Quantity = quantity;
        }

  
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateForm(out int quantity);
            if (isValid)
            {

                string enaValue = EanBox.Text.Trim();
                bool isEanExist = false;

                foreach(var itemProdcut in products)
                {
                    if (itemProdcut.Ean == enaValue)
                    {
                        isEanExist = true;
                        break;
                    }
                }

                if(isEanExist)
                {
                    MessageBox.Show("Produkt z takim kodem EAN już istnieje.");
                    return;
                }

              
                var product = new Product();
                FormToProduct(product, quantity);
                products.Add(product);

                ClearForm();
                RefreshGrid();
            }
        }
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = null;
            var selectedProductTmp = ProductsTable.SelectedItem;

            if (selectedProductTmp != null)
            {
                selectedProduct = (Product)selectedProductTmp;
            }
            else
            {
                MessageBox.Show("Najpierw wybierz produkt z tabeli.");
                return;
            }

             
            if (ValidateForm(out int quantity))
            {
                string eanValue = EanBox.Text.Trim();
                bool isEanExist = false;

                foreach (var itemProduct in products)
                {
                    if (itemProduct.Ean == eanValue && itemProduct != selectedProduct)
                    {
                        isEanExist = true;
                        break;
                    }
                }

                if (isEanExist)
                {
                    MessageBox.Show("Inny produkt ma już taki kod EAN.");
                    return;
                }

                FormToProduct(selectedProduct, quantity);

                ClearForm();
                RefreshGrid();
            }

        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {

            var selectedProductTmp = ProductsTable.SelectedItem;

            if(selectedProductTmp == null)
            {
                MessageBox.Show("Najpierw wybierz produkt z tabeli.");
                return;
            }

            Product selectedProduct = (Product)selectedProductTmp;

            var answer = MessageBox.Show($"Usunąć produkt \"{selectedProduct.Name}\"?",
                                         "Potwierdzenie", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.No) 
            { 
                return; 
            }

            products.Remove(selectedProduct);

            ClearForm();
            RefreshGrid();
        }

   
        private void ProductsTableSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsTable.SelectedItem is Product p)
            {
                EanBox.Text = p.Ean;
                NameBox.Text = p.Name;
                CategoryBox.Text = p.Category;
                SectorBox.Text = p.Sector;
                RackBox.Text = p.Rack;
                ShelfBox.Text = p.Shelf;
                QuantityBox.Text = p.Quantity.ToString();
            }
        }

        private void SearchBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshGrid();
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            ProductsTable.SelectedItem = null;
            EanBox.Text = NameBox.Text = CategoryBox.Text = "";
            SectorBox.Text = RackBox.Text = ShelfBox.Text = QuantityBox.Text = "";
        }
    }
}