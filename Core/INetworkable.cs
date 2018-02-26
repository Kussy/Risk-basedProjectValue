using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>接続能力を与える</summary>
    interface INetworkable
    {
        /// <summary>先行群</summary>
        IEnumerable<INetworkable> Parents { get; }
        /// <summary>後続群</summary>
        IEnumerable<INetworkable> Children { get; }

        /// <summary>先行する</summary>
        /// <param name="networkable">後続</param>
        void Precede(INetworkable networkable);
        /// <summary>後続する</summary>
        /// <param name="networkable">先行</param>
        void Succeed(INetworkable networkable);
        /// <summary>分岐する</summary>
        /// <param name="networkable">後続群</param>
        void Branch(IEnumerable<INetworkable> networkable);
        /// <summary>合流する</summary>
        /// <param name="networkable">先行群</param>
        void Merge(IEnumerable<INetworkable> networkable);
    }
}
