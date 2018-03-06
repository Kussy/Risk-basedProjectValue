using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>プロジェクト</summary>
    public class Project
    {
        /// <summary>アクティビティ群</summary>
        public IEnumerable<Activity> Activities { get; private set; } = Enumerable.Empty<Activity>();

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
            var value = RPVfinish().Value - Activities.Sum(a => a.ContributedValue().Value);
            var currency = Activities.First().Income.Currency;
            return Money.Of(value, currency);
        }

        /// <summary>完了時点でのRPVを求める</summary>
        /// <returns>完了時点のRPV</returns>
        public Money RPVfinish()
        {
            Contract.Requires(Activities != null);
            Contract.Requires(Activities.Count() != 0);
            Contract.Requires(Activities.All(a => a.Income.Currency == a.DirectCost.Currency));
            Contract.Requires(Activities.Select(a => a.Income.Currency).Distinct().Count() == 1);
            Contract.Requires(Activities.Select(a => a.DirectCost.Currency).Distinct().Count() == 1);
            var value = Activities.Sum(a => a.Income.Value - a.DirectCost.Value);
            var currency = Activities.First().Income.Currency;
            return Money.Of(value, currency);
        }
    }
}
