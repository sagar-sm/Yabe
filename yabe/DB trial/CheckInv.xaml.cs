using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Data;
using Npgsql;

namespace DB_trial
{
    /// <summary>
    /// Interaction logic for CheckInv.xaml
    /// </summary>
    public partial class CheckInv : Page
    {
        public CheckInv()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Page2.xaml", UriKind.Relative));
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            string query = String.Concat("SELECT qwe.ino, pname, qwe.price, date_time, pay_type, dealer from ((eshopping.invoice NATURAL JOIN eshopping.mode_of_payment) as qwe LEFT JOIN eshopping.product on eshopping.product.ino = qwe.ino) WHERE qwe.uid = ", GlobalClass.GlobalVar.ToString());
            NpgsqlCommand showInv = new NpgsqlCommand(query, conn);

            NpgsqlDataReader sw = showInv.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Load(sw);

            this.dataGrid1.AutoGenerateColumns = true;
            this.dataGrid1.ItemsSource = dt.DefaultView;
            sw.Close();
        }
    }
}
