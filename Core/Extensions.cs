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
    }
}