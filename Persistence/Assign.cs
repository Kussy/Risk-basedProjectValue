namespace Kussy.Analysis.Project.Persistence
{
    /// <summary></summary>
    public class Assign
    {
        /// <summary></summary>
        public int ActivityId { get; set; }
        /// <summary></summary>
        public Activity Activity { get; set; }
        /// <summary></summary>
        public int ResourceId { get; set; }
        /// <summary></summary>
        public Resource Resource { get; set; }
        /// <summary>量</summary>
        public decimal Quantity { get; set; }
    }
}