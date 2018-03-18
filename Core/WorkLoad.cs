using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>和</returns>
        public static WorkLoad operator +(WorkLoad x, WorkLoad y)
        {
            return Of(x.Value + y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>差</returns>
        public static WorkLoad operator -(WorkLoad x, WorkLoad y)
        {
            return Of(x.Value - y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static WorkLoad operator *(WorkLoad x, decimal y)
        {
            return Of(x.Value * y);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static WorkLoad operator *(decimal x, WorkLoad y)
        {
            return Of(x * y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static LeadTime operator /(WorkLoad x, IEnumerable<Resource> y)
        {
            Contract.Requires(!y.IsNullOrEmpty());
            Contract.Requires(y.All(r => r.Quantity != 0));
            Contract.Requires(y.All(r => r.Productivity != 0));
            return LeadTime.Of(x.Value / y.Sum(r => r.Quantity * r.Productivity));
        }
    }
}