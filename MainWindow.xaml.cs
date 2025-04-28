using Npgsql;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PracticaBd
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=localhost;Port=5432;Database=PractitcaBd;User Id=postgres;Password=admin";
        private int attemptCount = 0;
        private DateTime? lockoutEndTime = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (lockoutEndTime.HasValue && DateTime.Now < lockoutEndTime.Value)
            {
                MessageBox.Show("Вход заблокирован. Попробуйте снова через минуту.");
                return;
            }

            string login = loginTextBox.Text;
            string password = passwordBox.Password;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE login=@login AND password=@password", conn))
                {
                    cmd.Parameters.AddWithValue("login", login);
                    cmd.Parameters.AddWithValue("password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        attemptCount = 0;
                        ClglavnWindow clglavnWindow = new ClglavnWindow();
                        clglavnWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        attemptCount++;
                        MessageBox.Show("Проверьте корректность введенных данных");

                        if (attemptCount >= 3)
                        {
                            lockoutEndTime = DateTime.Now.AddMinutes(1);
                            MessageBox.Show("Вы превысили количество попыток входа. Попробуйте снова через минуту.");
                        }
                    }
                }
            }
        }
        private void RegistrButton_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regForm = new RegWindow();
            regForm.Show();
            this.Close();
        }

        private void RazrButton_Click(object sender, RoutedEventArgs e)
        {
            RazrWindow razrForm = new RazrWindow();
            razrForm.Show();
            this.Close();
        }
    }
}