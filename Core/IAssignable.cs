using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>資源配置能力を与える</summary>
    interface IAssignable
    {
        /// <summary>資源</summary>
        IEnumerable<Resource> Resources { get; }
        /// <summary>資源群を割当てる</summary>
        /// <param name="resources">資源群</param>
        void Assign(IEnumerable<Resource> resources);
        /// <summary>資源割当を解除する</summary>
        void UnAssign();
    }
}
