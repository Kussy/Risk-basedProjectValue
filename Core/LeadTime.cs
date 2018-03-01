using System;
using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>リードタイム</summary>
    public class LeadTime : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; }
        /// <summary>単位</summary>
        public TimeType Unit { get; private set; }

        /// <summary>プライベートコンストラクタ</summary>
        private LeadTime() { }
        /// <summary>コンストラクタ</summary>
        /// <param name="value">値</param>
        /// <param name="unit">単位</param>
        public LeadTime(decimal value, TimeType unit)
        {
            Value = value;
            Unit = unit;                
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