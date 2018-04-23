using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>コーディングを助ける拡張クラス</summary>
    public static class Extensions
    {
        /// <summary>セレクター用内部クラス</summary>
        /// <typeparam name="T">型</typeparam>
        /// <typeparam name="TKey">キー</typeparam>
        private sealed class CommonSelector<T, TKey> : IEqualityComparer<T>
        {
            private Func<T, TKey> _selector;

            public CommonSelector(Func<T, TKey> selector)
            {
                _selector = selector;
            }

            public bool Equals(T x, T y)
            {
                return _selector(x).Equals(_selector(y));
            }

            public int GetHashCode(T obj)
            {
                return _selector(obj).GetHashCode();
            }
        }

        /// <summary>Containsの引数にラムダを使えるようにする拡張メソッド</summary>
        /// <typeparam name="T">型</typeparam>
        /// <typeparam name="TKey">セレクターキー</typeparam>
        /// <param name="source">対象</param>
        /// <param name="value">要素</param>
        /// <param name="selector">セレクター</param>
        /// <returns>含まれる場合は真</returns>
        public static bool Contains<T, TKey>(this IEnumerable<T> source, T value, Func<T, TKey> selector)
        {
            return source.Contains(value, new CommonSelector<T, TKey>(selector));
        }
        
        /// <summary>対象の列挙可能型が空かどうかを判定する</summary>
                 /// <typeparam name="T">型</typeparam>
                 /// <param name="target">対象</param>
                 /// <returns>空の場合に真</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> target)
        {
            if (target == null) throw new ArgumentNullException();
            if (target is ICollection<T> generic) return generic.Count == 0;
            if (target is ICollection nonGeneric) return nonGeneric.Count == 0;
            return !target.Any();
        }

        /// <summary>対象の列挙可能型がNullまたは空かどうかを判定する</summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="target">対象</param>
        /// <returns>空の場合に真</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> target)
        {
            return target?.IsEmpty() ?? true;
        }

        /// <summary>対象がfrom以上to以下の範囲に入っているかを判定する</summary>
        /// <param name="target">対象</param>
        /// <param name="from">範囲下限</param>
        /// <param name="to">範囲上限</param>
        /// <returns></returns>
        public static bool IsRangeOf(this decimal target, decimal from, decimal to)
        {
            Contract.Requires(from <= to);
            return target >= from && target <= to;
        }

        /// <summary>Money型用の集計関数</summary>
        /// <param name="source">対象</param>
        /// <returns>集計結果</returns>
        public static Money Sum(this IEnumerable<Money> source)
        {
            var sum = Money.Of();
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>LeadTime型用の集計関数</summary>
        /// <param name="source">対象</param>
        /// <returns>集計結果</returns>
        public static LeadTime Sum(this IEnumerable<LeadTime> source)
        {
            var sum = LeadTime.Of();
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }
    }
}