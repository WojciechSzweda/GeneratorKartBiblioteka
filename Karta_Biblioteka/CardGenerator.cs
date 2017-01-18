using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    public static class CardGenerator
    {
        static Random rnd = new Random();
        private static string[] nameArray;
        private static string[] streetArray;
        private static string[] cityArray;

        public static void InitData()
        {
            StreamReader file = new StreamReader("Imiona.txt");
            string line = file.ReadLine();
            nameArray = line.Split(',');

            file = new StreamReader("Streets.txt");
            line = file.ReadToEnd();
            streetArray = line.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            file = new StreamReader("Miasta.txt");
            line = file.ReadLine();
            cityArray = line.Split(' ');

            file.Close();
        }


        public static void FillTable(SqlConnection conn)
        {
            using (SqlCommand command = new SqlCommand(@"INSERT INTO Karta (Imię, Nazwisko, Miejscowość, [Kod pocztowy], Ulica, [Nr domu], [Nr mieszkania],[Nr kontaktowy], [Data wydania]) 
                                                         OUTPUT INSERTED.ID VALUES (@imie, @nazwisko, @city, @kod, @ulica, @nrd, @nrm, @nrtel, @data)", conn))
            {
                command.Parameters.AddWithValue("@imie", GenerateName());
                command.Parameters.AddWithValue("@nazwisko", GenerateName());
                command.Parameters.AddWithValue("@city", GenerateCity());
                command.Parameters.AddWithValue("@kod", GeneratePostalCode());
                command.Parameters.AddWithValue("@ulica", GenerateStreet());
                command.Parameters.AddWithValue("@nrd", GenerateNrDomu());
                string nrM = GenerateNrM();
                if (nrM == null)
                {
                    command.Parameters.AddWithValue("@nrm", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@nrm", nrM);
                }
                command.Parameters.AddWithValue("@nrtel", GeneratePhoneNumber());
                command.Parameters.AddWithValue("@data", GenerateDate());
                command.ExecuteScalar();
            }

        }

        public static DateTime GenerateDate()
        {

            DateTime time = new DateTime(rnd.Next(2015, 2017), rnd.Next(1, 12), rnd.Next(1, 29));

            return time;
        }

        public static string GeneratePostalCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(rnd.Next(10, 99));
            sb.Append('-');
            sb.Append(rnd.Next(100, 999));
            return sb.ToString();
        }

        public static string GenerateNrDomu()
        {
            return rnd.Next(1, 99).ToString();
        }
        public static string GenerateNrM()
        {
            int nr = rnd.Next(1, 50);
            if (nr > 10)
            {
                return null;
            }
            else
            {
                return nr.ToString();
            }
        }

        public static string GenerateName()
        {
            return nameArray[rnd.Next(1, nameArray.Length)].Trim();
        }

        public static string GenerateStreet()
        {
            return streetArray[rnd.Next(1, streetArray.Length)];
        }

        public static string GenerateCity()
        {
            return cityArray[rnd.Next(1, cityArray.Length)];
        }

        public static string GeneratePhoneNumber()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                sb.Append(rnd.Next(1, 9));
            }
            return sb.ToString();
        }

    }
}
