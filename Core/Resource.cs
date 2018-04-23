using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>作業能力を与える</summary>
    public class Resource : ValueObject
    {
        /// <summary>ID</summary>
        public int Id { get; private set; }
        /// <summary>コード</summary>
        public string Code { get; private set; }
        /// <summary>名称</summary>
        public string Name { get; private set; }
        /// <summary>量</summary>
        public decimal Quantity { get; protected set; }
        /// <summary>生産性</summary>
        public decimal Productivity { get; protected set; }
        /// <summary>資源種類</summary>
        public ResourceType Type { get; protected set; }

        /// <summary>プライベートコンストラクタ</summary>
        protected Resource() { }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="code">コード</param>
        /// <param name="name">名称</param>
        /// <param name="quantity">量</param>
        /// <param name="productivity">生産性</param>
        /// <param name="resourceType">資源種類</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static Resource Of(string code = "", string name ="", decimal quantity = 0m, decimal productivity = 0m, ResourceType resourceType = ResourceType.Unknown)
        {
            Contract.Requires(quantity >= 0m);
            Contract.Requires(productivity >= 0m);

            return new Resource()
            {
                Code = code,
                Name = name,
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