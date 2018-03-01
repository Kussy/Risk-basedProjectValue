using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>抽象通貨クラス</summary>
    public class Money : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; }
        /// <summary>通貨</summary>
        public Currency Currency { get; private set; }

        /// <summary>プライベートコンストラクタ</summary>
        private Money() { }
        /// <summary>コンストラクタ</summary>
        /// <param name="value">値</param>
        /// <param name="currency">通貨</param>
        public Money(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }
        /// <summary>プロパティを反復処理する</summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Currency;
        }
    }
}