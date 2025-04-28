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
using System.Collections.ObjectModel;

namespace PracticaBd
{

    public partial class EmployeeWindow : Window
    {
        private DatabaseConnection dbConnection;
        public EmployeeWindow()
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
                string query = "SELECT employeeid AS КодРаботника, surname AS Фамилия, firstname AS Имя, lastname AS Отчество, post AS Должность FROM employee ORDER BY surname ASC";
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
            string post = postComboBox.Text;

            if (!string.IsNullOrWhiteSpace(surname) && !string.IsNullOrWhiteSpace(firstname) && !string.IsNullOrWhiteSpace(lastname) && !string.IsNullOrWhiteSpace(post))
            {
                AddEmployee(surname, firstname, lastname, post);
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
                int employeeId = Convert.ToInt32(selectedRow["КодРаботника"]);
                string surname = surnameComboBox.Text;
                string firstname = firstnameComboBox.Text;
                string lastname = lastnameComboBox.Text;
                string post = postComboBox.Text;

                if (!string.IsNullOrWhiteSpace(surname) && !string.IsNullOrWhiteSpace(firstname) && !string.IsNullOrWhiteSpace(lastname) && !string.IsNullOrWhiteSpace(post))
                {
                    UpdateEmployee(employeeId, surname, firstname, lastname, post);
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
                int employeeId = Convert.ToInt32(selectedRow["КодРаботника"]);
                DeleteEmployee(employeeId);
                LoadData();
            }
            else 
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
        }
        private void AddEmployee(string surname, string firstname, string lastname, string post)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO employee (surname, firstname, lastname, post) VALUES (@surname, @firstname, @lastname, @post)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@post", post);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateEmployee(int employeeId, string surname, string firstname, string lastname, string post)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE employee SET surname = @surname, firstname = @firstname, lastname = @lastname, post = @post WHERE employeeid = @employeeId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@surname", surname);
                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@post", post);
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteEmployee(int employeeId)
        {
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM employee WHERE employeeid = @employeeId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
