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
    public partial class ClglavnWindow : Window
    {
        public ClglavnWindow()
        {
            InitializeComponent();
        }
        private void ClientServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ClientServiceWindow clientserviceForm = new ClientServiceWindow();
            clientserviceForm.Show();
        }
        private void ClientSparePartButton_Click(object sender, RoutedEventArgs e)
        {
            ClientSparePartWindow clientsparePartForm = new ClientSparePartWindow();
            clientsparePartForm.Show();
        }
        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainForm = new MainWindow();
            mainForm.Show();
            this.Close();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            CartWindow cartForm = new CartWindow();
            cartForm.Show();
        }
    }
}
