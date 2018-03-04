using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>リードタイム</summary>
    public class LeadTime : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; } = 0m;
        /// <summary>単位</summary>
        public TimeType Unit { get; private set; } = TimeType.Day;

        /// <summary>プライベートコンストラクタ</summary>
        private LeadTime() { }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <returns>インスタンス初期値</returns>
        public static LeadTime Of()
        {
            return new LeadTime();
        }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="value">値</param>
        /// <param name="unit">単位</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static LeadTime Estemated(decimal value, TimeType unit)
        {
            Contract.Requires(value >= 0);
            return new LeadTime()
            {
                Value = value,
                Unit = unit,
            };
        }

        /// <summary>プロパティを反復処理する</summary>
        /// <returns>要素列挙</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Unit;
        }
    }
}