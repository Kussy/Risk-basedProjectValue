using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>作業量</summary>
    public class WorkLoad : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; } = 0m;

        /// <summary>プライベートコンストラクタ</summary>
        private WorkLoad() { }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <returns>インスタンス初期値</returns>
        public static WorkLoad Of()
        {
            return new WorkLoad();
        }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="value">値</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static WorkLoad Of(decimal value)
        {
            Contract.Requires(value >= 0);
            return new WorkLoad
            {
                Value = value,
            };
        }
        /// <summary>プロパティを反復処理する</summary>
        /// <returns>要素列挙</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}