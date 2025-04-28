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
    public partial class TypeWindow : Window
    {
        private DatabaseConnection dbConnection;
        public TypeWindow()
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
                string query = "SELECT typeid AS КодВида, name AS Название FROM type ORDER BY name ASC";
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

            if (!string.IsNullOrWhiteSpace(name))
            {
                AddType(name);
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
                int typeId = Convert.ToInt32(selectedRow["КодВида"]);
                string name = nameComboBox.Text;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    UpdateType(typeId, name);
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
                int typeId = Convert.ToInt32(selectedRow["КодВида"]);
                DeleteType(typeId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddType(string name)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO type (name) VALUES (@name)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateType(int typeId, string name)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE type SET name = @name WHERE typeid = @typeId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@typeId", typeId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteType(int typeId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM type WHERE typeid = @typeId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@typeId", typeId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
