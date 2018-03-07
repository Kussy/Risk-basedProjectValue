using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>リードタイム</summary>
    public class LeadTime : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; } = 0m;

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
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static LeadTime Of(decimal value)
        {
            Contract.Requires(value >= 0);
            return new LeadTime()
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

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>和</returns>
        public static LeadTime operator +(LeadTime x, LeadTime y)
        {
            return Of(x.Value + y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>差</returns>
        public static LeadTime operator -(LeadTime x, LeadTime y)
        {
            return Of(x.Value - y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static LeadTime operator *(LeadTime x, decimal y)
        {
            return Of(x.Value * y);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static LeadTime operator *(decimal x, LeadTime y)
        {
            return Of(x * y.Value);
        }
    }
}