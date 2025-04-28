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
    public partial class GlavnWindow : Window
    {
        public GlavnWindow()
        {
            InitializeComponent();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow employeeForm = new EmployeeWindow();
            employeeForm.Show();
        }
        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow clientForm = new ClientWindow();
            clientForm.Show();
        }
        private void BidButton_Click(object sender, RoutedEventArgs e)
        {
            BidWindow bidForm = new BidWindow();
            bidForm.Show();
        }
        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceForm = new ServiceWindow();
            serviceForm.Show();
        }
        private void ChecksButton_Click(object sender, RoutedEventArgs e)
        {
            ChecksWindow checksForm = new ChecksWindow();
            checksForm.Show();
        }
        private void SparePartButton_Click(object sender, RoutedEventArgs e)
        {
            SparePartWindow sparePartForm = new SparePartWindow();
            sparePartForm.Show();
        }
        private void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            TypeWindow typeForm = new TypeWindow();
            typeForm.Show();
        }
        private void UsersButton_Click(Object sender, RoutedEventArgs e) 
        {
            UsersWindow usersForm = new UsersWindow();
            usersForm.Show();
        }
        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainForm = new MainWindow();
            mainForm.Show();
            this.Close();
        }
    }
}
