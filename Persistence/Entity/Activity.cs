using System.Collections.Generic;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>アクティビティ</summary>
    public class Activity
    {
        /// <summary>ID</summary>
        public int Id { get; set; }
        /// <summary>コード</summary>
        public string Code { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>プロジェクトID</summary>
        public int ProjectId { get; set; }
        /// <summary>プロジェクト</summary>
        public Project Project { get; set; }
        /// <summary>状態</summary>
        public StateType State { get; set; }
        /// <summary>作業量</summary>
        public decimal Workload { get; set; }
        /// <summary>固定リードタイム</summary>
        public decimal FixedLeadTime { get; set; }
        /// <summary>収入</summary>
        public decimal Income { get; set; }
        /// <summary>外部支出</summary>
        public decimal ExternalCost { get; set; }
        /// <summary>失敗確率</summary>
        public decimal RateOfFailure { get; set; }
        /// <summary>資源割当</summary>
        public IEnumerable<Assign> Assigns { get; set; }
    }
}