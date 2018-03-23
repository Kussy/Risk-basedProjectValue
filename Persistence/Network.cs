namespace Kussy.Analysis.Project.Persistence
{
    /// <summary></summary>
    public class Network
    {
        /// <summary></summary>
        public int AncestorId { get; set; }
        /// <summary></summary>
        public Activity Ancestor { get; set; }
        /// <summary></summary>
        public int DescendantId { get; set; }
        /// <summary></summary>
        public Activity Descendant { get; set; }
        /// <summary></summary>
        public int Depth { get; set; }
    }
}