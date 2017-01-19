using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    public static class DBHelper
    {
        public static void DeleteTable(string tableName,SqlConnection con) {
            string sql = String.Format("DELETE [{0}]", tableName);
            new SqlCommand(sql, con).ExecuteNonQuery();
        }

        public static int[] getIdList(string tableName, SqlConnection conn)
        {
            string sql = String.Format("SELECT ID FROM {0}", tableName);
            var command = new SqlCommand(sql, conn);
            List<int> ids = new List<int>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())

                    ids.Add(reader.GetInt32(0));
            }
            return ids.ToArray();
        }

        public static void ExecuteInConnectionContext(SqlConnection conn, Action action) {
            conn.Open();
            action();
            conn.Close();

        }

    }
}
