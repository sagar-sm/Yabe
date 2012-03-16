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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> usernames = new List<string>();
        List<string> uid = new List<string>();
        string curuid;
        List<string> pwds = new List<string>();
        
        public MainWindow()
        {
            InitializeComponent();
                        
        }

        public static void connectnow()
       {

            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            
            conn.Open();
           
            //conn.Close();
        }
        

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connectnow();
                StatusLbl.Content = "Status: Connected!";
            }
            catch
            {
                StatusLbl.Content = "Status: Failed!";
            }
        }

        private void initLogins()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT email, pwd, uid FROM eshopping.client", conn);

            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                int i = 0;
                while (dr.Read())
                {
                    usernames.Add((string)dr[0]);
                    pwds.Add((string)dr[1]);
                    uid.Add((string)dr[2]);
                    i++;                 
                }                            

            }

            finally
            {
                conn.Close();
            }
        }

        private void SigninBtn_Click(object sender, RoutedEventArgs e)
        {
            string un = usernameTxt.Text;
            string pwd = passwordBox1.Password;
            initLogins();
            if (usernames.Contains(un))
            {
                int i = usernames.IndexOf(un);
                curuid = uid[i];
                MessageBox.Show(curuid.ToString());
                if (pwd.Equals((string)pwds[i]))
                {
                    
                }

            }
        }


       

    }
}
