using System;
using System.Collections.Generic;
using System.IO;

namespace TableRecords
{
    class Program
    {
        static void Main(string[] args)
        {
            Table<TestClass> table = new Table<TestClass>();

            var textStream = new StreamWriter(File.Create(@"C:\text.txt"));
            TestClass test = new TestClass()
            {
                AccountType = "account1",
                Bonuses = 12,
                DateOfBirth = "12.12.2012",
                FirstName = "Nikita",
                LastName = "Ivanov",
                Id = 1,
                Money = 123

            };

            TestClass test1 = new TestClass()
            {
                AccountType = "account2",
                Bonuses = 12,
                DateOfBirth = "12.12.2012",
                FirstName = "Pavel",
                LastName = "Sidorov",
                Id = 2,
                Money = 1321

            };
            List<TestClass> list = new List<TestClass>();
            list.Add(test);
            list.Add(test1);

            table.WriteTable(list, Console.Out);
            table.WriteTable(list, textStream);
        }
    }
}
