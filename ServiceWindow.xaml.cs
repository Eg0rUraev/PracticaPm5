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
    public partial class ServiceWindow : Window
    {
        private DatabaseConnection dbConnection;
        public ServiceWindow()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadData();
            LoadType();
        }
        public void LoadData()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT serviceid AS КодУслуги, typeid AS КодВида, name AS Название, price AS Цена FROM service ORDER BY name ASC";
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
        private void LoadType()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT typeid, name FROM type";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            typeComboBox.Items.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
                typeComboBox.DisplayMemberPath = "Name";
                typeComboBox.SelectedValuePath = "Id";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int typeId = (int)typeComboBox.SelectedValue;
            string name = nameComboBox.Text;
            string priceText = priceComboBox.Text;

            if (!string.IsNullOrWhiteSpace(name) && float.TryParse(priceText, out float price))
            {
                AddService(typeId, name, price);
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
                int serviceId = Convert.ToInt32(selectedRow["КодУслуги"]);
                int typeId = (int)typeComboBox.SelectedValue;
                string name = nameComboBox.Text;
                string priceText = priceComboBox.Text;

                if (!string.IsNullOrWhiteSpace(name) && float.TryParse(priceText, out float price))
                {
                    UpdateService(serviceId, typeId, name, price);
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
                int serviceId = Convert.ToInt32(selectedRow["КодУслуги"]);
                DeleteService(serviceId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddService(int typeId, string name, float price)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO service (typeid, name, price) VALUES (@typeId, @name, @price)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@typeId", typeId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateService(int serviceId, int typeId, string name, float price)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE service SET typeid = @typeId, name = @name, price = @price WHERE serviceid = @serviceId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@typeId", typeId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@serviceId", serviceId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteService(int serviceId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM service WHERE serviceid = @serviceId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@serviceId", serviceId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
