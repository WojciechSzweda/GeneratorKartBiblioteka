using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    public static class Publishers
    {
        public static void FillTable(SqlConnection conn, int maxInserts)
        {
            using (StreamReader reader = new StreamReader("Publisher.txt"))
            {
                for (int i = 0; i < maxInserts; i++)
                {
                    using (SqlCommand command = new SqlCommand(@"INSERT INTO Wydawca (Nazwa) 
                                                            VALUES (@name)", conn))
                    {
                        command.Parameters.AddWithValue("@name", reader.ReadLine());
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
