using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PracticaBd
{
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            CartListBox.Items.Clear();
            foreach (var item in CartService.Items)
            {
                CartListBox.Items.Add($"{item.Type}: {item.Name} - {item.Price} руб.");
            }
            TotalPriceTextBlock.Text = $"Итого: {CartService.GetTotalPrice()} руб.";
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (CartService.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }

            MessageBox.Show($"Заказ оформлен! Сумма: {CartService.GetTotalPrice()} руб.");
            CartService.Clear();
            Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CartListBox.SelectedItem == null) return;

            int selectedIndex = CartListBox.SelectedIndex;
            CartService.Items.RemoveAt(selectedIndex);
            LoadCartItems();
        }
    }
}
