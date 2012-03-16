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

using System.IO;
using System.Data;
using Npgsql;
using System.Windows.Markup;
using System.Globalization;
using System.Xml;

namespace DB_trial
{   
    
    
    /// <summary>
    /// Interaction logic for BuyPage.xaml
    /// </summary>
    public partial class BuyPage : Page
    {
        /*
        private int margin = 17, offset = 0;

        private const string _template0 =
            
            "<TextBlock xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Height=\"31\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" Name=\"link_";
        private const string _mtemplate1 =
            "\" Width=\"Auto\" Margin=\"55,";
        private const string _template1 = ",0,0\">" + "<Hyperlink NavigateUri=\"Cart.xaml\">";
                  
        private const string _template2 = "</Hyperlink>"+"</TextBlock>";
        private List<string> tempxml = new List<string>();
         */

        private List<string> cats = new List<string>();

        public BuyPage()
        {
            InitializeComponent();
            //Loaded += OnLoad;

        }

        /*
        private void selectCat(object sender, RoutedEventArgs e)
        {
            TextBlock snd = (TextBlock) sender;
            int i = Int16.Parse(snd.Name);
            i++;
            GlobalClass.GlobalVar = i;
        }
         */

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21;Port=5432;User Id=200901072;Password=12345;Database=200901072;");
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT cat_name FROM eshopping.category", conn);
            int i = 0;
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                
                while (dr.Read())
                {
                    cats.Add((string)dr[0]);
                    categoryBox1.ItemsSource = cats;
                    /*offset = 34 * (i);
                    int y = offset + margin;
                    tempxml.Add(String.Concat(_template0,i.ToString(),_mtemplate1, y.ToString(), _template1, cats[i], _template2));*/
                    i++;
                }

            }

            finally
            {
                conn.Close();
            }
        
        /*
        for(int cnt = 0; cnt < i; cnt++)
        {
            StringReader stringReader = new StringReader(tempxml[cnt]);
            XmlReader xmlReader = XmlReader.Create(stringReader); 
            UIElement tree = (UIElement)XamlReader.Load(xmlReader); 
            LayoutRoot.Children.Add(tree);
            

            TextBlock tbl = (TextBlock)XamlReader.Load(String.Format(CultureInfo.InvariantCulture,
                _template,
                _count++,
                (string) cats[cnt],
                String.Concat((string)cats[cnt], ".xaml"),
                (string)cats[cnt]));
            
        }
         */
        
        }

        List<Int16> ino = new List<Int16>();
        List<int> stock = new List<int>();

        private void categoryChanged(object sender, SelectionChangedEventArgs e)
        {
            int cid = categoryBox1.SelectedIndex + 1;
            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21; Port=5432; User Id=200901072; Password=12345; Database=200901072;");
            conn.Open();
            string query = String.Concat("SELECT ino, pname, price, stock FROM eshopping.product WHERE cat_id = ", cid.ToString());
                        

            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();

                DataTable dt = new DataTable();

                dt.Load(dr);

                this.dataGrid1.AutoGenerateColumns = true;
                this.dataGrid1.ItemsSource = dt.DefaultView; 
                
            }
            finally
            {
                conn.Close();
            }
            
        }

        private void atcBtn_Click(object sender, RoutedEventArgs e)
        {
            int reqqty = Int16.Parse(qtyTbx.Text);
            Int16 reqino = Int16.Parse(inoTxb.Text);
            

            NpgsqlConnection conn = new NpgsqlConnection("Server=10.100.71.21; Port=5432; User Id=200901072; Password=12345; Database=200901072;");
            conn.Open();
            NpgsqlCommand command1 = new NpgsqlCommand("SELECT ino, stock FROM eshopping.product", conn);
            
            NpgsqlDataReader dr = command1.ExecuteReader();

            while (dr.Read())
            {
                ino.Add((Int16)dr[0]);
                stock.Add((int)dr[1]);               
            }

            if (ino.Contains(reqino))
            {               
                int i = ino.IndexOf(reqino);
                if (stock[i] >= reqqty)
                {
                    string query = String.Concat("insert into eshopping.add_to_cart values ((select ino from eshopping.product where ino = ", reqino.ToString(), ") , ", GlobalClass.GlobalVar.ToString(), " , ", reqqty.ToString(), ", (select (price*", reqqty.ToString() , ") from eshopping.product where ino =", reqino.ToString(), " ) , ", "'2011-06-30 22:37:14'", " )");

                    //MessageBox.Show(query);
                    NpgsqlCommand command = new NpgsqlCommand(query, conn);
                    int rowsaffected;

                    try
                    {
                        rowsaffected = command.ExecuteNonQuery();
                        MessageBox.Show("Successfully Added Item to your Shopping Cart!");
                    }

                    finally
                    {
                        conn.Close();
                    }
                }
                else { MessageBox.Show("Sorry! Not Enough Items in Stock"); }

            }
           
            

           

        }

        private void proceed(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"CheckOut.xaml", UriKind.Relative));
        }
              



    }
}
