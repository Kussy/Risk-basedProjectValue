using System;
using System.Collections.Generic;
using System.Text;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>値オブジェクトの抽象クラス</summary>
    public abstract class ValueObject
    {
        /// <summary>等価比較</summary>
        /// <param name="left">左</param>
        /// <param name="right">右</param>
        /// <returns>等価な場合はtrue</returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        /// <summary>非等価比較</summary>
        /// <param name="left">左</param>
        /// <param name="right">右</param>
        /// <returns>非等価な場合はtrue</returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        /// <summary></summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetAtomicValues();

        /// <summary>等価比較</summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>等価な場合はtrue</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            ValueObject other = (ValueObject)obj;
            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^
                    ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }

                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }
        // Other utilility methods
    }
}
