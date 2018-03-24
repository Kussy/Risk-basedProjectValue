using System.Collections.Generic;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>プロジェクト</summary>
    public class Project
    {
        /// <summary>ID</summary>
        public int Id { get; set; }
        /// <summary>コード</summary>
        public string Code { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>通貨単位</summary>
        public CurrencyType UnitOfCurrency { get; set; }
        /// <summary>時間単位</summary>
        public TimeType UnitOfTime { get; set; }
        /// <summary>プロジェクト期間</summary>
        public decimal Term { get; private set; }
        /// <summary>予算</summary>
        public decimal Budjet { get; private set; }
        /// <summary>単位時間辺りの遅延損害金</summary>
        public decimal LiquidatedDamages { get; set; }
        /// <summary>アクティビティ群</summary>
        public ICollection<Activity> Activities { get; set; }
    }
}