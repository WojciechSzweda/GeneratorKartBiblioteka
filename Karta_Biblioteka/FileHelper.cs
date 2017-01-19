using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Karta_Biblioteka
{
    static class FileHelper
    {
        public static string[] ReadLines(string fileName,int offset,int numberOfLines) {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                for (int i = 0; i < offset; i++)
                {
                    if (reader.EndOfStream) return lines.ToArray();
                    reader.ReadLine();
                }

                for (int i = 0; i < numberOfLines; i++)
                {
                    if (reader.EndOfStream) break;
                    lines.Add(reader.ReadLine());
                }
            }
                return lines.ToArray();
        }
        public static string[] ReadLines(string fileName, int numberOfLines)
        {
            return ReadLines(fileName, 0, numberOfLines);
        }


        }
}
