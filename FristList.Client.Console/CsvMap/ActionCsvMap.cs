using CsvHelper.Configuration;
using FristList.Dto;

namespace FristList.Client.Console.CsvMap
{
    public sealed class ActionCsvMap : ClassMap<Action>
    {
        internal ActionCsvMap()
        {
            Map(a => a.Id).Name("ActionId");
            Map(a => a.Description).Name("ActionDescription");
            Map(a => a.StartTime).Name("ActionStartTime");
            Map(a => a.EndTime).Name("ActionEndTime");
            Map(a => a.Categories).TypeConverter<CategoryArrayTypeCsvConverter>()
                .Name("Categories");
        }
    }
}