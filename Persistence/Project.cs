using System;
using System.Collections.Generic;
using System.Text;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary></summary>
    public class Project
    {
        /// <summary></summary>
        public int Id { get; set; }
        /// <summary></summary>
        public string Code { get; set; }
        /// <summary></summary>
        public string Name { get; set; }
        /// <summary></summary>
        public CurrencyType UnitOfCurrency { get; set; }
        /// <summary></summary>
        public TimeType UnitOfTime { get; set; }
        /// <summary>プロジェクト期間</summary>
        public decimal Term { get; private set; }
        /// <summary>予算</summary>
        public decimal Badjet { get; private set; }
        /// <summary>遅延存在金</summary>
        public decimal LiquidatedDamages { get; set; }
        /// <summary></summary>
        public ICollection<Activity> Activities { get; set; }
    }
}