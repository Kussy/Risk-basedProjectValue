namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>ネットワーク</summary>
    public class Network
    {
        /// <summary>先祖ID</summary>
        public int AncestorId { get; set; }
        /// <summary>先祖</summary>
        public Activity Ancestor { get; set; }
        /// <summary>子孫ID</summary>
        public int DescendantId { get; set; }
        /// <summary>子孫</summary>
        public Activity Descendant { get; set; }
        /// <summary>深さ</summary>
        public int Depth { get; set; }
    }
}