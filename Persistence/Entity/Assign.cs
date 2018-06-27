namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>資源割当</summary>
    public class Assign
    {
        /// <summary>アクティビティID</summary>
        public string ActivityId { get; set; }
        /// <summary>アクティビティ</summary>
        public Activity Activity { get; set; }
        /// <summary>資源ID</summary>
        public string ResourceId { get; set; }
        /// <summary>資源</summary>
        public Resource Resource { get; set; }
        /// <summary>量</summary>
        public decimal Quantity { get; set; }
    }
}