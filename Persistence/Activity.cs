using System.Collections.Generic;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary></summary>
    public class Activity
    {
        /// <summary></summary>
        public int Id { get; set; }
        /// <summary></summary>
        public string Code { get; set; }
        /// <summary></summary>
        public string Name { get; set; }
        /// <summary></summary>
        public int ProjectId { get; set; }
        /// <summary></summary>
        public Project Project { get; set; }
        /// <summary></summary>
        public State State { get; set; }
        /// <summary>作業量</summary>
        public decimal Workload { get; set; }
        /// <summary>固定時間</summary>
        public decimal FixedLeadTime { get; set; }
        /// <summary>収入</summary>
        public decimal Income { get; set; }
        /// <summary>支出</summary>
        public decimal ExternalCost { get; set; }
        /// <summary>失敗確率</summary>
        public decimal RateOfFailure { get; set; }
        /// <summary>資源</summary>
        public IEnumerable<Assign> Assigns { get; set; }
    }
}