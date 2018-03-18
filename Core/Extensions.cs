using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}