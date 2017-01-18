using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    public static class BookGenerator
    {
        public static void FillTable(SqlConnection conn, int maxInserts)
        {
            Random rnd = new Random();
            StreamReader readerTitles = new StreamReader("Books\\Title.txt");
            StreamReader readerISBN = new StreamReader("Books\\ISBN.txt");
            StreamReader readerYear = new StreamReader("Books\\YearOfPublish.txt");
            var pubIds = getIdList(conn);
            for (int i = 0; i < maxInserts; i++)
            {
                using (SqlCommand command = new SqlCommand(@"inSert into Książka (Tytuł, ISBN, [Rok wydania], [ID_Wydawca]) output inserted.id
                                                            values (@title, @ISBN, @year, @pubID)", conn))
                {
                    command.Parameters.AddWithValue("@title", readerTitles.ReadLine());
                    command.Parameters.AddWithValue("@ISBN", readerISBN.ReadLine());
                    command.Parameters.AddWithValue("@year", readerYear.ReadLine());
                    command.Parameters.AddWithValue("@pubID", pubIds[rnd.Next(1,pubIds.Length)]);
                    command.ExecuteScalar();
                }
            }
        }

        private static int[] getIdList(SqlConnection conn)
        {
            string sql = String.Format("SELECT ID FROM Wydawca");
            var command = new SqlCommand(sql, conn);
            List<int> ids = new List<int>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())

                    ids.Add(reader.GetInt32(0));
            }
            return ids.ToArray();
        }
    }
}
