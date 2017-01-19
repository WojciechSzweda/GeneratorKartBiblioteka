using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Karta_Biblioteka
{
    public static class BookGenerator
    {
        public static void FillTableAuthors(SqlConnection conn, int maxInserts) {

            DataTable authorTable = new DataTable();
            authorTable.Columns.Add("Podpis",typeof(string));            
            foreach (var author in FileHelper.ReadLines("Author.txt", maxInserts))
            {
                authorTable.Rows.Add(author);
            }
            var bulkCopy = new SqlBulkCopy(conn) {
                DestinationTableName = "Autor"
            };
            bulkCopy.ColumnMappings.Add("Podpis","Podpis");
            bulkCopy.WriteToServer(authorTable);

        }
        public static void ConnectBooksAndAuthors(SqlConnection conn) {
            DBHelper.DeleteTable("Autorzy",conn);
            var authorIds = DBHelper.getIdList("Autor", conn);
            var bookIds = DBHelper.getIdList("Książka", conn);
            DataTable autorsTable = new DataTable();
            autorsTable.Columns.Add("ID_Książka",typeof(int));
            autorsTable.Columns.Add("ID_Autor", typeof(int));
            Random random = new Random();

            foreach (var bookId in bookIds)
            {
                if (random.NextDouble() > 0.9)
                {
                    foreach (var author in authorIds.randomElements(2))
                    {
                        autorsTable.Rows.Add(bookId, author);
                    }
                }
                else
                {
                    autorsTable.Rows.Add(bookId, authorIds.randomElement());
                }
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn) {
                DestinationTableName = "Autorzy"
            };
            bulkCopy.ColumnMappings.Add("ID_Książka", "ID_Książka");
            bulkCopy.ColumnMappings.Add("ID_Autor", "ID_Autor");
            bulkCopy.WriteToServer(autorsTable);

        }
        public static void FillCategories(SqlConnection conn) {
            DataTable categoriesTable = new DataTable();
            categoriesTable.Columns.Add("Nazwa",typeof(string));
            var categories = File.ReadAllLines("bookCategories.txt");
            foreach (var category in categories)
            {
                categoriesTable.Rows.Add(category);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
            {
                DestinationTableName = "Kategoria"
            };
            bulkCopy.ColumnMappings.Add("Nazwa", "Nazwa");
            bulkCopy.WriteToServer(categoriesTable);
        }

       public static void ConnectBooksAndCategories(SqlConnection conn){
            DBHelper.DeleteTable("Kategoria-Książka", conn);
            var categoriesId = DBHelper.getIdList("Autor", conn);
            var bookIds = DBHelper.getIdList("Książka", conn);
            DataTable categoryTable = new DataTable();
            categoryTable.Columns.Add("ID_Książka", typeof(int));
            categoryTable.Columns.Add("ID_Kategoria", typeof(int));

            Random random = new Random();
            foreach (var bookId in bookIds)
            {

                if (random.NextDouble() > 0.7)
                {
                    foreach (var categoryId in categoriesId.randomElements(2))
                    {
                        categoryTable.Rows.Add(bookId, categoryId);
                    }
                }
                else { 
                    categoryTable.Rows.Add(bookId, categoriesId.randomElement());
                }
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
            {
                DestinationTableName = "[Kategoria-Książka]"
            };
            bulkCopy.ColumnMappings.Add("ID_Książka", "ID_Książka");
            bulkCopy.ColumnMappings.Add("ID_Kategoria", "ID_Kategoria");
            bulkCopy.WriteToServer(categoryTable);
        }


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
