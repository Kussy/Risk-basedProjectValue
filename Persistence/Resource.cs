using System.Collections.Generic;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>資源</summary>
    public class Resource 
    {
        /// <summary>ID</summary>
        public int Id { get; set; }
        /// <summary>コード</summary>
        public string Code { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>資源種類</summary>
        public ResourceType Type { get; set; }
        /// <summary>生産性</summary>
        public decimal Productivity { get; set; }
        /// <summary>資源割当</summary>
        public IEnumerable<Assign> Assigns { get; set; }
    }
}