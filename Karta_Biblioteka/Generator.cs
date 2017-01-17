using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    public static class Generator
    {
        static Random rnd = new Random();



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
            string line;
            StreamReader file = new StreamReader("Imiona.txt");
            line = file.ReadLine();
            //Console.WriteLine(line);
            var nameArray = line.Split(',');
            int nameArraySize = nameArray.Length;
            return nameArray[rnd.Next(1, nameArraySize)].Trim();
        }

        public static string GenerateStreet()
        {
            int streetArraySize = 135;
            var file = new StreamReader("Streets.txt");
            var streetArray = new string[streetArraySize];
            for (int i = 0; i < streetArraySize; i++)
            {
                streetArray[i] = file.ReadLine();
            }

            return streetArray[rnd.Next(1, streetArraySize)];
        }

        public static string GenerateCity()
        {
            string line;
            var file = new StreamReader("Miasta.txt");
            line = file.ReadLine();
            var cityArray = line.Split(' ');
            int cityArraySize = cityArray.Length;

            return cityArray[rnd.Next(1, cityArraySize)];
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
