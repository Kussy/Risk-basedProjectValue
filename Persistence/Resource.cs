using System.Collections.Generic;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>作業能力を与える</summary>
    public class Resource 
    {
        /// <summary>ID</summary>
        public int Id { get; set; }
        /// <summary></summary>
        public string Code { get; set; }
        /// <summary></summary>
        public string Name { get; set; }
        /// <summary>資源種類</summary>
        public ResourceType Type { get; set; }
        /// <summary>生産性</summary>
        public decimal Productivity { get; set; }
        /// <summary>生産性</summary>
        public IEnumerable<Assign> Assigns { get; set; }
    }
}