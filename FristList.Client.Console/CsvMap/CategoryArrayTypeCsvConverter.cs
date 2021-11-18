using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FristList.Dto;

namespace FristList.Client.Console.CsvMap
{
    public class CategoryArrayTypeCsvConverter : TypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == string.Empty)
                return Array.Empty<Category>();

            var names = text.Split(",");
            var categories = names.Select(name => new Category
            {
                Name = name
            });

            return categories.ToArray();
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var categories = (IReadOnlyList<Category>) value;
            if (categories.Count == 0)
                return string.Empty;

            return string.Join(",", categories.Select(c => c.Name));
        }
    }
}