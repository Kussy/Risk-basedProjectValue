using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>資源配置能力を与える</summary>
    interface IAssignable
    {
        /// <summary>資源プロパティ</summary>
        IEnumerable<IWorkable> Resources { get; }
        /// <summary>資源群を与えて資源プロパティを更新する</summary>
        /// <param name="resources">資源群</param>
        void Assign(IEnumerable<IWorkable> resources);
    }
}
