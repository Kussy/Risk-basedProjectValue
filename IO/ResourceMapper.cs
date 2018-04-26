using CsvHelper.Configuration;
using Kussy.Analysis.Project.Persistence;

namespace Kussy.Analysis.Project.IO
{
    /// <summary>資源クラスとのマッピング定義</summary>
    public class ResourceMapper : ClassMap<Resource>
    {
        /// <summary></summary>
        private int IndexForId => 0;
        /// <summary></summary>
        private int IndexForCode => 1;
        /// <summary></summary>
        private int IndexForName => 2;
        /// <summary></summary>
        private int IndexForType => 3;
        /// <summary></summary>
        private int IndexForProductivity => 4;

        /// <summary></summary>
        private string NameForId => "Id";
        /// <summary></summary>
        private string NameForCode => "Code";
        /// <summary></summary>
        private string NameForName => "Name";
        /// <summary></summary>
        private string NameForType => "Type";
        /// <summary></summary>
        private string NameForProductivity => "Productivity";


        /// <summary>コンストラクタ</summary>
        public ResourceMapper()
        {
            Map(x => x.Id)
                .Name(NameForId)
                .Index(IndexForId)
                .Validate(f => string.IsNullOrEmpty(f) || int.TryParse(f, out var i))
                .Default(-1);
            Map(x => x.Code)
                .Name(NameForCode)
                .Index(IndexForCode)
                .Validate(f => !string.IsNullOrEmpty(f));
            Map(x => x.Name)
                .Name(NameForName)
                .Index(IndexForName)
                .Validate(f => !string.IsNullOrEmpty(f));
            Map(x => x.Type)
                .Name(NameForType)
                .Index(IndexForType)
                .Validate(f => string.IsNullOrEmpty(f) || (int.TryParse(f, out var i) && i >= 0 && i <= 2))
                .Default(ResourceType.Unknown);
            Map(x => x.Productivity)
                .Name(NameForProductivity)
                .Index(IndexForProductivity)
                .Validate(f => string.IsNullOrEmpty(f) || (decimal.TryParse(f, out var d) && d > 0))
                .Default(1.0m);
        }
    }
}