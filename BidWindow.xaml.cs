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
    public partial class BidWindow : Window
    {
        private DatabaseConnection dbConnection;
        public BidWindow()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadData();
            LoadClient();
            LoadEmployee();
        }
        public void LoadData()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM bid ORDER BY bidid ASC";
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
        private void LoadClient() 
        {
            using (var connection = dbConnection.GetConnection()) 
            {
                connection.Open();
                string query = "SELECT clientid, surname FROM client";
                using (var command = new NpgsqlCommand(query, connection)) 
                {
                    using (var reader = command.ExecuteReader()) 
                    {
                        while (reader.Read()) 
                        {
                            clientComboBox.Items.Add(new { Id = reader.GetInt32(0), Surname = reader.GetString(1) });
                        }
                    }
                }
                clientComboBox.DisplayMemberPath = "Surname";
                clientComboBox.SelectedValuePath = "Id";
            }
        }
        private void LoadEmployee()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT employeeid, surname FROM employee";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeComboBox.Items.Add(new { Id = reader.GetInt32(0), Surname = reader.GetString(1) });
                        }
                    }
                }
                employeeComboBox.DisplayMemberPath = "Surname";
                employeeComboBox.SelectedValuePath = "Id";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int clientId = (int)clientComboBox.SelectedValue;
            int employeeId = (int)employeeComboBox.SelectedValue;
            DateTime datebid;
            string descriptionproblem = descriptionproblemComboBox.Text;

            if (DateTime.TryParse(datebidComboBox.Text, out datebid) && !string.IsNullOrWhiteSpace(descriptionproblem))
            {
                AddBid(clientId, employeeId, datebid, descriptionproblem);
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
                int bidId = Convert.ToInt32(selectedRow["bidid"]);
                int clientId = (int)clientComboBox.SelectedValue;
                int employeeId = (int)employeeComboBox.SelectedValue;
                DateTime datebid;
                string descriptionproblem = descriptionproblemComboBox.Text;

                if (DateTime.TryParse(datebidComboBox.Text, out datebid) && !string.IsNullOrWhiteSpace(descriptionproblem))
                {
                    UpdateBid(bidId, clientId, employeeId, datebid, descriptionproblem);
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
                int bidId = Convert.ToInt32(selectedRow["bidid"]);
                DeleteBid(bidId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddBid(int clientId, int employeeId, DateTime datebid, string descriptionproblem)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO bid (clientid, employeeid, datebid, descriptionproblem) VALUES (@clientId, @employeeId, @datebid, @descriptionproblem)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.Parameters.AddWithValue("@datebid", datebid);
                    command.Parameters.AddWithValue("@descriptionproblem", descriptionproblem);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateBid(int bidId, int clientId, int employeeId, DateTime datebid, string descriptionproblem)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE bid SET clientid = @clientId, employeeid = @employeeId, datebid = @datebid, descriptionproblem = @descriptionproblem WHERE bidid = @bidId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.Parameters.AddWithValue("@datebid", datebid);
                    command.Parameters.AddWithValue("@descriptionproblem", descriptionproblem);
                    command.Parameters.AddWithValue("@bidId", bidId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteBid(int bidId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM bid WHERE bidid = @bidId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bidId", bidId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
