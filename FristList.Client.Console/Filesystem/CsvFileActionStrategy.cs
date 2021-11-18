using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FristList.Client.Console.CsvMap;

namespace FristList.Client.Console.Filesystem
{
    public class CsvFileActionStrategy : IFileImportActionStrategy, IFileExportActionStrategy
    {
        private readonly CsvConfiguration _configuration;
        private readonly Action<CsvContext> _contextInitializer;
        
        public CsvFileActionStrategy(CsvConfiguration configuration = null, Action<CsvContext> contextInitializer = null)
        {
            configuration ??= new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true
            };
            
            _contextInitializer = contextInitializer;
            _configuration = configuration;
        }
        
        public IEnumerable<Dto.Action> Import(string path)
        {
            using var reader = new StreamReader(path);
            using var csvReader = new CsvReader(reader, _configuration);
            _contextInitializer?.Invoke(csvReader.Context);

            csvReader.Context.RegisterClassMap<ActionCsvMap>();
            return csvReader.GetRecords<Dto.Action>()
                .ToArray();
        }

        public void Export(IEnumerable<Dto.Action> actions, string path)
        {
            using var writer = new StreamWriter(path);
            using var csvWriter = new CsvWriter(writer, _configuration);
            _contextInitializer?.Invoke(csvWriter.Context);
            
            csvWriter.WriteRecords(actions);
        }
    }
}