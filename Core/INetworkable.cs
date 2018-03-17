using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>接続能力を与える</summary>
    public interface INetworkable
    {
        /// <summary>先行群</summary>
        IEnumerable<INetworkable> Parents { get; }
        /// <summary>後続群</summary>
        IEnumerable<INetworkable> Children { get; }
        /// <summary>先祖</summary>
        IEnumerable<INetworkable> Ancestors { get; }
        /// <summary>子孫</summary>
        IEnumerable<INetworkable> Descendants { get; }

        /// <summary>先行する</summary>
        /// <param name="child">後続</param>
        void Precede(INetworkable child);
        /// <summary>後続する</summary>
        /// <param name="parent">先行</param>
        void Succeed(INetworkable parent);
        /// <summary>分岐する</summary>
        /// <param name="children">後続群</param>
        void Branch(IEnumerable<INetworkable> children);
        /// <summary>合流する</summary>
        /// <param name="parents">先行群</param>
        void Merge(IEnumerable<INetworkable> parents);
        /// <summary>関係を断つ</summary>
        /// <param name="related">関係者</param>
        void Remove(INetworkable related);
    }
}
