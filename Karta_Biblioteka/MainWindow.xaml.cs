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

        void FillTable(SqlConnection conn)
        {
            try
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(@"INSERT INTO Karta (Imię, Nazwisko, Miejscowość, [Kod pocztowy], Ulica, [Nr domu], [Nr mieszkania],[Nr kontaktowy], [Data wydania]) 
                                                         OUTPUT INSERTED.ID VALUES (@imie, @nazwisko, @city, @kod, @ulica, @nrd, @nrm, @nrtel, @data)", conn))
                {
                    command.Parameters.AddWithValue("@imie", Generator.GenerateName());
                    command.Parameters.AddWithValue("@nazwisko", Generator.GenerateName());
                    command.Parameters.AddWithValue("@city", Generator.GenerateCity());
                    command.Parameters.AddWithValue("@kod", Generator.GeneratePostalCode());
                    command.Parameters.AddWithValue("@ulica", Generator.GenerateStreet());
                    command.Parameters.AddWithValue("@nrd", Generator.GenerateNrDomu());
                    string nrM = Generator.GenerateNrM();
                    if (nrM == null)
                    {
                        command.Parameters.AddWithValue("@nrm", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@nrm", nrM);
                    }
                    command.Parameters.AddWithValue("@nrtel", Generator.GeneratePhoneNumber());
                    command.Parameters.AddWithValue("@data", Generator.GenerateDate());
                    command.ExecuteScalar();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(tbConnString.Text);
            btnAdd.IsEnabled = true;
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
            for (int i = 0; i < int.Parse(tbQuantity.Text); i++)
            {
                FillTable(conn);
            }
        }

        private void tbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
