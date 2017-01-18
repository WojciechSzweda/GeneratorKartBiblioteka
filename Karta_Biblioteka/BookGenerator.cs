using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Karta_Biblioteka
{
    public static class BookGenerator
    {
        public static void FillTableBooks(SqlConnection conn, int maxInserts)
        {
            Random rnd = new Random();
            using (StreamReader readerTitles = new StreamReader("Books\\Title.txt"),
            readerISBN = new StreamReader("Books\\ISBN.txt"),
            readerYear = new StreamReader("Books\\YearOfPublish.txt"))
            {
                var pubIds = DBHelper.getIdList("Wydawca", conn);
                for (int i = 0; i < maxInserts; i++)
                {
                    using (SqlCommand command = new SqlCommand(@"insert into Książka (Tytuł, ISBN, [Rok wydania], [ID_Wydawca]) 
                                                            values (@title, @ISBN, @year, @pubID)", conn))
                    {
                        command.Parameters.AddWithValue("@title", readerTitles.ReadLine());
                        command.Parameters.AddWithValue("@ISBN", readerISBN.ReadLine());
                        var year = readerYear.ReadLine();
                        if (year == "0")
                        {
                            command.Parameters.AddWithValue("@year", "2000"); 
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@year", year);

                        }
                        command.Parameters.AddWithValue("@pubID", pubIds[rnd.Next(0, pubIds.Length - 1)]);
                        command.ExecuteNonQuery();

                    }
                }
            }
        }

        public static void FillTableCopies(SqlConnection conn, int max)
        {
            Random rnd = new Random();
            var books = DBHelper.getIdList("Książka", conn);
            for (int i = 0; i < max; i++)
            {
                using (SqlCommand command = new SqlCommand(@"INSERT INTO Kopia (ID_Książka, [Stan Książki]) 
                                                        VALUES (@BookID, @Stan)", conn))
                {
                    command.Parameters.AddWithValue("@BookID", books[rnd.Next(0,books.Length-1)]);
                    command.Parameters.AddWithValue("@Stan", StateGenerator(rnd.Next(1,100)));
                    command.ExecuteNonQuery();
                }
            }
        }

        private static int StateGenerator(int rand)
        {
            if (rand < 50)
            {
                return 1;
            }
            else if (rand < 75)
            {
                return 2;
            }
            else if (rand < 90)
            {
                return 3;
            }
            else return 4;
        }


    }
}
