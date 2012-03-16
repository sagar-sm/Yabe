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
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Page
    {
        public CheckOut()
        {
            InitializeComponent();
        }

        List<string> modes = new List<string>();
        List<string> dealers = new List<string>();
        Int16 modeID = 0;
        Int16 TID = (Int16)(Application.Current as App).tid;

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            header.Content = "Your Shopping Cart";
            string query = String.Concat("SELECT pname,ino, qty, value_of_cart from eshopping.add_to_cart NATURAL JOIN eshopping.product where uid = ", GlobalClass.GlobalVar.ToString());
            NpgsqlCommand showCart = new NpgsqlCommand(query, conn);

            NpgsqlDataReader sw = showCart.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Load(sw);

            this.dataGrid1.AutoGenerateColumns = true;
            this.dataGrid1.ItemsSource = dt.DefaultView;
            sw.Close();

            NpgsqlCommand command = new NpgsqlCommand("SELECT DISTINCT pay_type FROM eshopping.mode_of_payment", conn);
            NpgsqlCommand command2 = new NpgsqlCommand("SELECT DISTINCT dealer FROM eshopping.mode_of_payment", conn);

            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    modes.Add((string)dr[0]);
                    
                }
                
                dr.Close();

                NpgsqlDataReader dr2 = command2.ExecuteReader();

                while (dr2.Read())
                {                    
                    dealers.Add((string)dr2[0]);
                }
                paymentCombo1.ItemsSource = dealers;
                paymentCombo2.ItemsSource = modes;
                dr2.Close();

            }
            
            finally
            {
                conn.Close();
            }
        }

        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            string paymode = paymentCombo2.SelectedValue.ToString();
            string dealermode = paymentCombo1.SelectedValue.ToString();

            string query = String.Concat("SELECT mod_id from eshopping.mode_of_payment WHERE pay_type = \'", paymode, "\' AND dealer = \'", dealermode, "\'");

            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand(query, conn);


            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();

                int i = 0;
                while (dr.Read())
                {
                    modeID = (Int16)dr[0];
                    i++;
                }

                if (i == 0)
                {
                    MessageBox.Show("Sorry! This payment Mode is currently not available. We're working to collaborate with the Banks soon. \nPlease select some other Payment Option");
                }
                else
                {
                    header.Content = "Your Invoice:";

                    string query2 = String.Concat("DELETE from eshopping.add_to_cart WHERE uid = ", GlobalClass.GlobalVar.ToString());

                    NpgsqlCommand deletion = new NpgsqlCommand(query2, conn);

                    NpgsqlDataReader dl = deletion.ExecuteReader();
                    dl.Close();
                    NpgsqlCommand updateInv = new NpgsqlCommand(String.Concat("UPDATE eshopping.invoice SET mod_id = ", modeID.ToString(), "WHERE uid = ", GlobalClass.GlobalVar.ToString()), conn);
                    dl = updateInv.ExecuteReader();
                    dl.Close();
                    MessageBox.Show("Payment Successful! We'll deliver the product to your place within 3 days.");
                }
                dr.Close();
            }
            finally
            {
 
            }

            

        }

    }

}