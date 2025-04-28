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
    public partial class ChecksWindow : Window
    {
        private DatabaseConnection dbConnection;
        public ChecksWindow()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            LoadData();
            LoadBid();
            LoadService();
            LoadSparePart();
        }
        public void LoadData()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM checks ORDER BY checkid ASC";
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
        private void LoadBid()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT bidid, descriptionproblem FROM bid";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bidComboBox.Items.Add(new { Id = reader.GetInt32(0), Descriptionproblem = reader.GetString(1) });
                        }
                    }
                }
                bidComboBox.DisplayMemberPath = "Descriptionproblem";
                bidComboBox.SelectedValuePath = "Id";
            }
        }
        private void LoadService()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT serviceid, name FROM service";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            serviceComboBox.Items.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
                serviceComboBox.DisplayMemberPath = "Name";
                serviceComboBox.SelectedValuePath = "Id";
            }
        }
        private void LoadSparePart()
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT sparepartid, name FROM sparepart";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sparepartComboBox.Items.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
                sparepartComboBox.DisplayMemberPath = "Name";
                sparepartComboBox.SelectedValuePath = "Id";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int bidId = (int)bidComboBox.SelectedValue;
            int serviceId = (int)serviceComboBox.SelectedValue;
            int sparepartId = (int)sparepartComboBox.SelectedValue;
            int quantityservice = int.TryParse(quantityserviceComboBox.Text, out int q) ? q : 0;
            int quantitysparepart = int.TryParse(quantitysparepartComboBox.Text, out int qua) ? qua : 0;
            string priceText = priceComboBox.Text;

            if (quantityservice > 0 && quantitysparepart > 0 && float.TryParse(priceText, out float price))
            {
                AddChecks(bidId, serviceId, sparepartId, quantityservice, quantitysparepart, price);
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
                int checkId = Convert.ToInt32(selectedRow["checkid"]);
                int bidId = (int)bidComboBox.SelectedValue;
                int serviceId = (int)serviceComboBox.SelectedValue;
                int sparepartId = (int)sparepartComboBox.SelectedValue;
                int quantityservice = int.TryParse(quantityserviceComboBox.Text, out int q) ? q : 0;
                int quantitysparepart = int.TryParse(quantitysparepartComboBox.Text, out int qua) ? qua : 0;
                string priceText = priceComboBox.Text; ;

                if (quantityservice > 0 && quantitysparepart > 0 && float.TryParse(priceText, out float price))
                {
                    UpdateChecks(checkId, bidId, serviceId, sparepartId, quantityservice, quantitysparepart, price);
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
                int checkId = Convert.ToInt32(selectedRow["checkid"]);
                DeleteChecks(checkId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddChecks(int bidId, int serviceId, int sparepartId, int quantityservice, int quantitysparepart, float price)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO checks (bidid, serviceid, sparepartid, quantityservice, quantitysparepart, price) VALUES (@bidId, @serviceId, @sparepartId, @quantityservice, @quantitysparepart, @price)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bidId", bidId);
                    command.Parameters.AddWithValue("@serviceId", serviceId);
                    command.Parameters.AddWithValue("@sparepartId", sparepartId);
                    command.Parameters.AddWithValue("@quantityservice", quantityservice);
                    command.Parameters.AddWithValue("@quantitysparepart", quantitysparepart);
                    command.Parameters.AddWithValue("@price", price);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateChecks(int checkId, int bidId, int serviceId, int sparepartId, int quantityservice, int quantitysparepart, float price)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE checks SET bidid = @bidId, serviceid = @serviceId, sparepartid = @sparepartId, quantityservice = @quantityservice, quantitysparepart = @quantitysparepart, price = @price WHERE checkid = @checkId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bidId", bidId);
                    command.Parameters.AddWithValue("@serviceId", serviceId);
                    command.Parameters.AddWithValue("@sparepartId", sparepartId);
                    command.Parameters.AddWithValue("@quantityservice", quantityservice);
                    command.Parameters.AddWithValue("@quantitysparepart", quantitysparepart);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@checkId", checkId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteChecks(int checkId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM checks WHERE checkid = @checkId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@checkId", checkId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
