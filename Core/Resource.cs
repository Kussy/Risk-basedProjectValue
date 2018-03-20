using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>作業能力を与える</summary>
    public class Resource : ValueObject
    {
        /// <summary>量</summary>
        public decimal Quantity { get; protected set; }
        /// <summary>生産性</summary>
        public decimal Productivity { get; protected set; }
        /// <summary>資源種類</summary>
        public ResourceType Type { get; protected set; }

        /// <summary>プライベートコンストラクタ</summary>
        protected Resource() { }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="quantity">量</param>
        /// <param name="productivity">生産性</param>
        /// <param name="resourceType">資源種類</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static Resource Of(decimal quantity = 0m, decimal productivity = 0m, ResourceType resourceType = ResourceType.Unknown)
        {
            Contract.Requires(quantity >= 0m);
            Contract.Requires(productivity >= 0m);

            return new Resource()
            {
                Quantity = quantity,
                Productivity = productivity,
                Type = resourceType,
            };
        }

        /// <summary>プロパティを反復処理する</summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Quantity;
            yield return Productivity;
        }
    }
}