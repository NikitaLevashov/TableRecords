using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TableRecords
{
    public class Table<T>
    {
        private const char Plus = '+';
        private const char VerticalBorder = '|';
        private const char HorisontalBorder = '-';
        private const char WhiteSpace = ' ';

        private enum RowType
        {
            Header,
            HorisontalBorderLine,
            Values,
        }

        PropertyInfo[] properties = typeof(T).GetProperties();

        StringBuilder sb = new StringBuilder();

        public void WriteTable(IEnumerable<T> records, TextWriter text)
        {
            foreach (var t in ToTableFormat(records))
            {
                text.WriteLine(t);
            }

            text.Flush();
        }

        private IEnumerable<string> ToTableFormat(IEnumerable<T> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException();
            }

            var columnLenght = this.MaxLenghtOffFields(records);
            var createrHeader = true;

            foreach (var item in records)
            {
                if (createrHeader)
                {
                    yield return ToRow(item, columnLenght, RowType.HorisontalBorderLine);
                    yield return ToRow(item, columnLenght, RowType.Header);
                    yield return ToRow(item, columnLenght, RowType.HorisontalBorderLine);
                    createrHeader = false;
                }

                yield return this.ToRow(item, columnLenght, RowType.Values);
                yield return this.ToRow(item, columnLenght, RowType.HorisontalBorderLine);
            }
        }

        private string ToRow(T record, Dictionary<string, int> columnLenght, RowType type)
        {
            this.sb.Clear();

            char symbol = (type == RowType.Header || type == RowType.Values) ? WhiteSpace : HorisontalBorder;
            char border = (type == RowType.Header || type == RowType.Values) ? VerticalBorder : Plus;

            foreach (var prop in this.properties)
            {
                var mappingProp = this.properties.First(x => x.Name == prop.Name);

                string value = type == RowType.Header ? prop.Name : (type == RowType.Values) ? prop.GetValue(record).ToString() :
                    new string(HorisontalBorder, columnLenght[prop.Name]);

                if (prop.PropertyType.IsValueType)
                {
                    this.sb.Append($"{border}{value.PadRight(columnLenght[prop.Name], symbol)}");
                }
                else
                {
                    this.sb.Append($"{border}{value.PadLeft(columnLenght[prop.Name], symbol)}");
                }
            }

            sb.Append($"{border}");

            return sb.ToString();
        }

        private Dictionary<string, int> MaxLenghtOffFields(IEnumerable<T> records)
        {
            var propertyLenghtPairs = new Dictionary<string, int>();
   
            int count = 0;

            foreach (var prop in properties)
            {
                foreach (var rec in records)
                {
                    propertyLenghtPairs.Add(count++.ToString(), prop.Name.Length);
                }
                var t = prop.Name;

                propertyLenghtPairs.Add(prop.Name, prop.Name.Length);
            }

            return propertyLenghtPairs;
        }
    }
}
