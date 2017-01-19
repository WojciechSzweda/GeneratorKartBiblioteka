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

        static Regex integerRegex = new Regex("[^0-9]+");


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
            btnAddCopies.IsEnabled = true;
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

        private void integerOnlyPreview(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = integerRegex.IsMatch(e.Text);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => DBHelper.DeleteTable("Karta", conn));

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => {
                DBHelper.DeleteTable("Oddanie", conn);
                DBHelper.DeleteTable("Wypożyczenie", conn);

            });
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => new BorowAndReturnFiller(conn).fillBorwosAndReturns(int.Parse(numberOfBorows.Text)));
        }

        private void btnAddPub_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => Publishers.FillTable(conn, int.Parse(tbQuantityPub.Text)));
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => BookGenerator.FillTableBooks(conn, int.Parse(tbQuantityBook.Text)));
        }

        private void btnAddCopies_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => BookGenerator.FillTableCopies(conn, int.Parse(tbQuantityCopies.Text)));
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => BookGenerator.FillTableAuthors(conn, int.Parse(tbQuantityAuthors.Text)));
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => BookGenerator.ConnectBooksAndAuthors(conn));
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => BookGenerator.FillCategories(conn));

        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.ExecuteInConnectionContext(conn, () => BookGenerator.ConnectBooksAndCategories(conn));
        }
    }
}
