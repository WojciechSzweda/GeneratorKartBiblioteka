using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    public static class Extensions
    {
        static Random random = new Random();

        public static T randomElement<T>(this T[] table) {
            return table[random.Next(table.Length)];
        }

        public static T[] randomElements<T>(this T[] table,int numberOfElements)
        {
            return table.OrderBy(x => Guid.NewGuid()).Take(numberOfElements).ToArray();
        }

    }
}
