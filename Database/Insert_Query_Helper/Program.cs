using System;
using System.IO;
using System.Collections.Generic;

namespace Insert_Query_Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            string date = DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss") ;
            string table;
            List<string> fields = new List<string>();
            Console.WriteLine("Enter table name");
            table = Console.ReadLine();
            Console.WriteLine("Enter column name (Enter 'STOP' to stop)");
            string input = Console.ReadLine();
            while (input != "STOP")
            {
                fields.Add(input);
                Console.WriteLine("Enter new column name (Enter 'STOP' to stop)");
                input = Console.ReadLine();
            }

            using (StreamWriter outputFile = new StreamWriter(@$"D:\MenU_Final_Project\Database\Insert_{date}.txt"))
            {
                string option = "yes";
                while(option == "y" || option == "yes")
                {
                    List<string> values = new List<string>();
                    for (int i = 0; i < fields.Count; i++)
                    {
                        Console.WriteLine($"Enter value for column {fields[i]}");
                        values.Add("'" + Console.ReadLine() + "'");
                    }
                    outputFile.WriteLine($"INSERT INTO {table} ({String.Join(',', fields)}) Values ({String.Join(',', values)});");
                    Console.WriteLine("Add another entry? Enter y or yes");
                    option = Console.ReadLine().ToLower();
                }
                
            }
            
        }
    }
}
