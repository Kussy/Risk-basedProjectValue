﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>金銭クラス</summary>
    public class Money : ValueObject, IComparable<Money>
    {
        /// <summary>値</summary>
        public decimal Value { get; protected set; }

        /// <summary>プライベートコンストラクタ</summary>
        protected Money() { }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="value">値</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static Money Of(decimal value = 0m)
        {
            Contract.Requires(value >= 0);

            return new Money()
            {
                Value = value,
            };
        }

        /// <summary>プロパティを反復処理する</summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        /// <summary>比較</summary>
        /// <param name="other">比較対象</param>
        /// <returns>比較結果</returns>
        public int CompareTo(Money other)
        {
            if (other == null) return 1;
            if (Value > other.Value) return 1;
            if (Value == other.Value) return 0;
            return -1;
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>和</returns>
        public static Money operator +(Money x, Money y)
        {
            return Of(x.Value + y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>差</returns>
        public static Money operator -(Money x, Money y)
        {
            return Of(x.Value - y.Value);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static Money operator *(Money x, decimal y)
        {
            return Of(x.Value * y);
        }

        /// <summary>演算子のオーバーロード</summary>
        /// <param name="x">1項</param>
        /// <param name="y">2項</param>
        /// <returns>積</returns>
        public static Money operator *(decimal x, Money y)
        {
            return Of(x * y.Value);
        }
    }
}