using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PracticaBd
{
    public partial class ClientSparePartWindow : Window
    {
        private DatabaseConnection dbConnection;
        public ClientSparePartWindow()
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
                string query = "SELECT sparepartid AS КодЗапчасти, name AS Название, price AS Цена FROM sparepart ORDER BY name ASC";
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
                MessageBox.Show("Выберите запчасть!");
                return;
            }
            DataRowView selectedRow = (DataRowView)DataGrid.SelectedItem;
            CartService.AddItem(
                id: Convert.ToInt32(selectedRow["КодЗапчасти"]),
                name: selectedRow["Название"].ToString(),
                price: Convert.ToSingle(selectedRow["Цена"]),
                type: "Запчасть"
            );
            MessageBox.Show("Запчасть добавлена в корзину!");
        }
        private void ViewCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow();
            cartWindow.Show();
        }
    }
}