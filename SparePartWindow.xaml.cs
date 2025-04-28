using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class SparePartWindow : Window
    {
        private DatabaseConnection dbConnection;
        public SparePartWindow()
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = nameComboBox.Text;
            string priceText = priceComboBox.Text;

            if (!string.IsNullOrWhiteSpace(name) && float.TryParse(priceText, out float price))
            {
                AddSparePart(name, price);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные данные");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)DataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int sparepartId = Convert.ToInt32(selectedRow["КодЗапчасти"]);
                string name = nameComboBox.Text;
                string priceText = priceComboBox.Text;

                if (!string.IsNullOrWhiteSpace(name) && float.TryParse(priceText, out float price))
                {
                    UpdateSparePart(sparepartId, name, price);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для обновления");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)DataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int sparepartId = Convert.ToInt32(selectedRow["КодЗапчасти"]);
                DeleteSparePart(sparepartId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddSparePart(string name, float price)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO sparepart (name, price) VALUES (@name, @price)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateSparePart(int sparepartId, string name, float price)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE sparepart SET name = @name, price = @price WHERE sparepartid = @sparepartId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@sparepartId", sparepartId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteSparePart(int sparepartId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM sparepart WHERE sparepartid = @sparepartId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sparepartId", sparepartId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
