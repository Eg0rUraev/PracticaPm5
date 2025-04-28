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
    public partial class ClientWindow : Window
    {
        private DatabaseConnection dbConnection;
        public ClientWindow()
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
                string query = "SELECT clientid AS КодКлиента, surname AS Фамилия, firstname AS Имя, lastname AS Отчество, telephonnumber AS НомерТелефона, cardata AS ДанныеОбАвтомобиле FROM client ORDER BY surname ASC";
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
            string surname = surnameComboBox.Text;
            string firstname = firstnameComboBox.Text;
            string lastname = lastnameComboBox.Text;
            string telephonnumber = telephonnumberComboBox.Text;
            string cardata = cardataComboBox.Text;

            if (!string.IsNullOrWhiteSpace(surname) && !string.IsNullOrWhiteSpace(firstname) && !string.IsNullOrWhiteSpace(lastname) && !string.IsNullOrWhiteSpace(telephonnumber) && !string.IsNullOrWhiteSpace(cardata))
            {
                AddClient(surname, firstname, lastname, telephonnumber, cardata);
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
                int clientId = Convert.ToInt32(selectedRow["КодКлиента"]);
                string surname = surnameComboBox.Text;
                string firstname = firstnameComboBox.Text;
                string lastname = lastnameComboBox.Text;
                string telephonnumber = telephonnumberComboBox.Text;
                string cardata = cardataComboBox.Text;

                if (!string.IsNullOrWhiteSpace(surname) && !string.IsNullOrWhiteSpace(firstname) && !string.IsNullOrWhiteSpace(lastname) && !string.IsNullOrWhiteSpace(telephonnumber) && !string.IsNullOrWhiteSpace(cardata))
                {
                    UpdateClient(clientId, surname, firstname, lastname, telephonnumber, cardata);
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
                int clientId = Convert.ToInt32(selectedRow["КодКлиента"]);
                DeleteClient(clientId);
                LoadData();
            }
            else 
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddClient(string surname, string firstname, string lastname, string telephonnumber, string cardata)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO client (surname, firstname, lastname, telephonnumber, cardata) VALUES (@surname, @firstname, @lastname, @telephonnumber, @cardata)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@telephonnumber", telephonnumber);
                    command.Parameters.AddWithValue("@cardata", cardata);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateClient(int clientId, string surname, string firstname, string lastname, string telephonnumber, string cardata)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE client SET surname = @surname, firstname = @firstname, lastname = @lastname, telephonnumber = @telephonnumber, cardata = @cardata WHERE clientid = @clientId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@telephonnumber", telephonnumber);
                    command.Parameters.AddWithValue("@cardata", cardata);
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteClient(int clientId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM client WHERE clientid = @clientId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
