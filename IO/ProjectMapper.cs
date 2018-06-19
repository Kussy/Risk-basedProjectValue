using CsvHelper.Configuration;
using Kussy.Analysis.Project.Persistence;

namespace Kussy.Analysis.Project.IO
{
    /// <summary>資源クラスとのマッピング定義</summary>
    public class ProjectMapper : ClassMap<Persistence.Project>
    {
        /// <summary></summary>
        private int IndexForId => 0;
        /// <summary></summary>
        private int IndexForCode => 1;
        /// <summary></summary>
        private int IndexForName => 2;
        /// <summary></summary>
        private int IndexForUnitOfCurrency => 3;
        /// <summary></summary>
        private int IndexForUnitOfTime => 4;
        /// <summary></summary>
        private int IndexForTerm => 5;
        /// <summary></summary>
        private int IndexForBudjet => 6;
        /// <summary></summary>
        private int IndexForLiquidatedDamages => 7;

        /// <summary></summary>
        private string NameForId => "Id";
        /// <summary></summary>
        private string NameForCode => "Code";
        /// <summary></summary>
        private string NameForName => "Name";
        /// <summary></summary>
        private string NameForUnitOfCurrency => "UnitOfCurrency";
        /// <summary></summary>
        private string NameForUnitOfTime => "UnitOfTime";
        /// <summary></summary>
        private string NameForTerm => "Term";
        /// <summary></summary>
        private string NameForBudjet => "Budjet";
        /// <summary></summary>
        private string NameForLiquidatedDamages => "LiquidatedDamages";


        /// <summary>コンストラクタ</summary>
        public ProjectMapper()
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
            Map(x => x.UnitOfCurrency)
                .Name(NameForUnitOfCurrency)
                .Index(IndexForUnitOfCurrency)
                .Validate(f => string.IsNullOrEmpty(f) || (int.TryParse(f, out var i) && i >= 0 && i <= 3))
                .Default(CurrencyType.JPY);
            Map(x => x.UnitOfTime)
                .Name(NameForUnitOfTime)
                .Index(IndexForUnitOfTime)
                .Validate(f => string.IsNullOrEmpty(f) || (int.TryParse(f, out var i) && i >= 0 && i <= 7))
                .Default(TimeType.Day);
            Map(x => x.Term)
                .Name(NameForTerm)
                .Index(IndexForTerm)
                .Validate(f => string.IsNullOrEmpty(f) || (decimal.TryParse(f, out var d) && d >= 0))
                .Default(0m);
            Map(x => x.Budjet)
                .Name(NameForBudjet)
                .Index(IndexForBudjet)
                .Validate(f => string.IsNullOrEmpty(f) || (decimal.TryParse(f, out var d) && d >= 0))
                .Default(0m);
            Map(x => x.LiquidatedDamages)
                .Name(NameForLiquidatedDamages)
                .Index(IndexForLiquidatedDamages)
                .Validate(f => string.IsNullOrEmpty(f) || (decimal.TryParse(f, out var d) && d >= 0))
                .Default(0m);
        }
    }
}