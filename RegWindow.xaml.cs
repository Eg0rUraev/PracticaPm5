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
    public partial class RegWindow : Window
    {
        private DatabaseConnection dbConnection;
        public RegWindow()
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

        private void RegistrButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginComboBox.Text;
            string password = passwordComboBox.Text;

            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                if (IsLoginExists(login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует. Пожалуйста, выберите другой логин.");
                    return;
                }

                AddUsers(login, password);
                LoadData();
                MessageBox.Show("Вы успешно зарегистрированы");
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные данные");
            }
        }

        private bool IsLoginExists(string login)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM users WHERE login = @login";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    var count = (long)command.ExecuteScalar();
                    return count > 0;
                }
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

        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainForm = new MainWindow();
            mainForm.Show();
            this.Close();
        }
    }
}
