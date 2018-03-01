using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>作業量</summary>
    public class WorkLoad : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; }
        /// <summary>時間種別</summary>
        public TimeType TimeUnit { get; private set; }
        /// <summary>作業者種別</summary>
        public WorkerType WorkerUnit { get; private set; }

        /// <summary>プライベートコンストラクタ</summary>
        private WorkLoad() { }
        /// <summary>コンストラクタ</summary>
        /// <param name="value">値</param>
        /// <param name="timeUnit">時間種別</param>
        /// <param name="workerUnit">作業者種別</param>
        public WorkLoad(decimal value, TimeType timeUnit, WorkerType workerUnit)
        {
            Value = value;
            TimeUnit = timeUnit;
            WorkerUnit = workerUnit;
        }
        /// <summary>プロパティを反復処理する</summary>
        /// <returns>要素列挙</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return TimeUnit;
            yield return WorkerUnit;
        }
    }
}