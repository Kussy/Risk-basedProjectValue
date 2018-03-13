using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>プロジェクト</summary>
    public class Project
    {
        /// <summary>アクティビティ群</summary>
        public IEnumerable<Activity> Activities { get; private set; } = Enumerable.Empty<Activity>();


        /// <summary>アクティビティ追加</summary>
        /// <param name="activities">アクティビティ群</param>
        public void AddActivities(params Activity[] activities)
        {
            Activities = Activities.Union(activities);
        }

        /// <summary>アクティビティ追加</summary>
        /// <param name="activities">アクティビティ群</param>
        public void AddActivities(IEnumerable<Activity> activities)
        {
            Activities = Activities.Union(activities);
        }

        /// <summary>アクティビティ削除</summary>
        /// <param name="activities">アクティビティ群</param>
        public void RemoveActivities(IEnumerable<Activity> activities)
        {
            Activities = Activities.Except(activities);
        }

        /// <summary>開始時点でのRPVを求める</summary>
        /// <returns>開始時点のRPV</returns>
        public Money RPVstart()
        {
            Contract.Requires(Activities != null);
            Contract.Requires(Activities.Count() != 0);
            var value = Activities.Sum(a => a.ExpectedCachFlow().Value);
            return Money.Of(value);
        }

        /// <summary>完了時点でのRPVを求める</summary>
        /// <returns>完了時点のRPV</returns>
        public Money RPVfinish()
        {
            Contract.Requires(Activities != null);
            Contract.Requires(Activities.Count() != 0);
            var value = Activities.Sum(a => a.Income.Value - a.DirectCost.Value);
            return Money.Of(value);
        }

        /// <summary>現時点でのキャッシュフローを求める</summary>
        /// <returns>現時点のキャッシュフロー</returns>
        public Money RPV()
        {
            Contract.Requires(Activities != null);
            Contract.Requires(Activities.Count() != 0);
            var accumulatedCF = Activities
                .Where(a => (a as Activity).State == State.Done)
                .Sum(a => (a as Activity).PrimevalCashFlow().Value);
            var futureCF = Activities
                .Where(a => (a as Activity).State != State.Done)
                .Sum(a => (a as Activity).ExpectedCachFlow().Value);
            return Money.Of(accumulatedCF + futureCF);
        }
    }
}
