using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
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
using Npgsql;

namespace PracticaBd
{
    public partial class ClientServiceWindow : Window
    {
        private DatabaseConnection dbConnection;
        public ClientServiceWindow()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadData();
        }
        public void LoadData()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT serviceid AS КодУслуги, name AS Название, price AS Цена FROM service ORDER BY name ASC";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        DataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }
        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу!");
                return;
            }

            DataRowView selectedRow = (DataRowView)DataGrid.SelectedItem;
            CartService.AddItem(
                id: Convert.ToInt32(selectedRow["КодУслуги"]),
                name: selectedRow["Название"].ToString(),
                price: Convert.ToSingle(selectedRow["Цена"]),
                type: "Услуга"
            );

            MessageBox.Show("Услуга добавлена в корзину!");
        }
        private void ViewCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow();
            cartWindow.Show();
        }
    }
}
