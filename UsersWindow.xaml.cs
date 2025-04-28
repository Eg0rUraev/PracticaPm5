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
    public partial class UsersWindow : Window
    {
        private DatabaseConnection dbConnection;
        public UsersWindow()
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
                string query = "SELECT userid AS КодПользователя, login AS Логин, password AS Пароль FROM users ORDER BY login ASC";
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
            string login = loginComboBox.Text;
            string password = passwordComboBox.Text;

            if (!string.IsNullOrWhiteSpace(login) &&!string.IsNullOrWhiteSpace(password))
            {
                AddUsers(login, password);
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
                int userId = Convert.ToInt32(selectedRow["КодПользователя"]);
                string login = loginComboBox.Text;
                string password = passwordComboBox.Text;

                if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
                {
                    UpdateUsers(userId, login, password);
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
                int userId = Convert.ToInt32(selectedRow["КодПользователя"]);
                DeleteUsers(userId);
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddUsers(string login, string password)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO users (login, password) VALUES (@login, @password)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateUsers(int userId, string login, string password)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE users SET login = @login, password = @password WHERE userid = @userId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteUsers(int userId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM users WHERE userid = @userId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
