using CsvHelper;
using Kussy.Analysis.Project.Persistence;
using System.Collections.Generic;
using System.IO;

namespace Kussy.Analysis.Project.IO
{
    /// <summary></summary>
    public sealed class RpvCsvReader
    {
        /// <summary>シングルトンインスタンス</summary>
        private static RpvCsvReader _instance = new RpvCsvReader();
        /// <summary>シングルトンプロパティ</summary>
        public static RpvCsvReader Instance => _instance;
        /// <summary>プライベートコンストラクタ</summary>
        private RpvCsvReader() { }

        /// <summary>資源ファイルの読込</summary>
        /// <param name="csvFilePath">CSVファイルパス</param>
        /// <returns>資源群</returns>
        public IEnumerable<Resource> GetResources(string csvFilePath)
        {
            using (var streamReader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(streamReader))
            {
                csv.Configuration.RegisterClassMap<ResourceMapper>();
                return csv.GetRecords<Resource>();
            }
        }
    }
}