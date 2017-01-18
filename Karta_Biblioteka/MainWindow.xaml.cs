using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Karta_Biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;

        


        public MainWindow()
        {
            InitializeComponent();
            CardGenerator.InitData();
        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(tbConnString.Text);
            btnAdd.IsEnabled = true;
            btnAddBook.IsEnabled = true;
            btnAddPub.IsEnabled = true;
            try
            {
                conn.Open();
                conn.Close();
            }
            catch (Exception exc)
            {
                tbConnError.Text = exc.Message;
                throw;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            for (int i = 0; i < int.Parse(tbQuantity.Text); i++)
            {
                CardGenerator.FillTable(conn);
            }
            conn.Close();
        }

        private void tbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            DBHelper.DeleteTable("Karta",conn);
            conn.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            DBHelper.DeleteTable("Oddanie", conn);
            DBHelper.DeleteTable("Wypożyczenie", conn);
            conn.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            new BorowAndReturnFiller(conn).fillBorwosAndReturns(int.Parse(numberOfBorows.Text));
            conn.Close();
        }

        private void btnAddPub_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            Publishers.FillTable(conn, int.Parse(tbQuantityPub.Text));
            conn.Close();
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            BookGenerator.FillTable(conn, int.Parse(tbQuantityBook.Text));
            conn.Close();
        }
    }
}
