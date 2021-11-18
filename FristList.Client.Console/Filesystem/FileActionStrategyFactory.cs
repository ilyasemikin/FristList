using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace FristList.Client.Console.Filesystem
{
    public class FileActionStrategyFactory : IFileActionStrategyFactory
    {
        private readonly Action<CsvContext> _csvContextInitializer;

        public FileActionStrategyFactory()
        {
            _csvContextInitializer = context =>
            {
                context.TypeConverterOptionsCache.AddOptions<DateTime>(
                    new TypeConverterOptions
                    {
                        Formats = new[]
                        {
                            "dd MMM yyyy HH:mm"
                        }
                    });
            };
        }

        protected CsvConfiguration CreateCsvConfiguration()
            => new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true
            };

        public IFileImportActionStrategy CreateImportActionStrategy(string name)
        {
            return name switch
            {
                "csv" => new CsvFileActionStrategy(CreateCsvConfiguration(), _csvContextInitializer),
                _ => throw new ArgumentException()
            };
        }

        public IFileExportActionStrategy CreateExportActionStrategy(string name)
        {
            return name switch
            {
                "csv" => new CsvFileActionStrategy(CreateCsvConfiguration(), _csvContextInitializer),
                _ => throw new ArgumentException()
            };
        }
    }
}