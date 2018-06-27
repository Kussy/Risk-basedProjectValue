namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>ネットワーク</summary>
    public class Network
    {
        /// <summary>先祖ID</summary>
        public string AncestorId { get; set; }
        /// <summary>先祖</summary>
        public Activity Ancestor { get; set; }
        /// <summary>子孫ID</summary>
        public string DescendantId { get; set; }
        /// <summary>子孫</summary>
        public Activity Descendant { get; set; }
        /// <summary>深さ</summary>
        public int Depth { get; set; }
    }
}