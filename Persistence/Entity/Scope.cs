namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>スコープ</summary>
    public class Scope
    {
        /// <summary>プロジェクトID</summary>
        public string ProjectId { get; set; }
        /// <summary>プロジェクト</summary>
        public Project Project { get; set; }
        /// <summary>アクティビティID</summary>
        public string ActivityId { get; set; }
        /// <summary>アクティビティ</summary>
        public Activity Activity { get; set; }
    }
}