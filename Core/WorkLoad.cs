using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>作業量</summary>
    public class WorkLoad : ValueObject
    {
        /// <summary>値</summary>
        public decimal Value { get; private set; } = 0m;
        /// <summary>時間種別</summary>
        public TimeType TimeUnit { get; private set; } = TimeType.Day;
        /// <summary>作業者種別</summary>
        public WorkerType WorkerUnit { get; private set; } = WorkerType.Human;

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
        /// <param name="timeUnit">時間種別</param>
        /// <param name="workerUnit">作業者種別</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static WorkLoad Of(decimal value, TimeType timeUnit, WorkerType workerUnit)
        {
            Contract.Requires(value >= 0);
            return new WorkLoad
            {
                Value = value,
                TimeUnit = timeUnit,
                WorkerUnit = workerUnit
            };
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