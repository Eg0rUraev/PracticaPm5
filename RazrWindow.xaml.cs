using Npgsql;
using System;
using System.Collections.Generic;
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
    public partial class RazrWindow : Window
    {
        private string connectionString = "Server=localhost;Port=5432;Database=PractitcaBd;User Id=postgres;Password=admin";
        public RazrWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordBox.Password;
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE login=@login AND password=@password", conn))
                {
                    cmd.Parameters.AddWithValue("login", login);
                    cmd.Parameters.AddWithValue("password", password);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0 && ((login == "adm1" && password == "1") || (login == "adm2" && password == "2")))
                    {
                        GlavnWindow glavnWindow = new GlavnWindow();
                        glavnWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Проверьте корректность введенных данных");
                    }
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
