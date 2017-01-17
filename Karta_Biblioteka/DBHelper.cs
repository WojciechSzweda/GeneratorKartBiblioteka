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
            string sql = String.Format("DELETE {0}", tableName);
            new SqlCommand(sql, con).ExecuteNonQuery();
        }



    }
}
