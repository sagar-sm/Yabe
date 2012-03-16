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
    /// Interaction logic for SellPage.xaml
    /// </summary>
    public partial class SellPage : Page
    {
        public SellPage()
        {
            InitializeComponent();
        }

        string pname;
        double mbid;
        List<string> cats = new List<string>();
        int cat;
        int AINO = (Int16)(Application.Current as App).aino;

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT cat_name FROM eshopping.category", conn);

            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    cats.Add((string)dr[0]);
                    comboBox1.ItemsSource = cats;
                }

            }

            finally
            {
                conn.Close();
            }
        
        }

        private void catchanged(object sender, SelectionChangedEventArgs e)
        {
            cat = comboBox1.SelectedIndex + 1;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            AINO++;
            string query = String.Concat("INSERT into eshopping.p_auction values ( " , AINO.ToString(), " , ", "'2011-11-17 02:15:00' , ", mbid.ToString(), " , ", "0, ", GlobalClass.GlobalVar.ToString(), " , ", cat.ToString(), " , '10.100.2.3' , '", pname, "')");  
            NpgsqlCommand command = new NpgsqlCommand(query, conn);

            try
            {
                NpgsqlDataReader writer = command.ExecuteReader();
            }
            finally
            {
                conn.Close();
            }

        }

        private void pnamechanged(object sender, TextChangedEventArgs e)
        {
            pname = textBox1.Text;
        }

        private void mbidchanged(object sender, TextChangedEventArgs e)
        {
            mbid = Double.Parse(textBox3.Text);
        }

    }
}
