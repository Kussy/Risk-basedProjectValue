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
        /// <summary>プロジェクトの単位通貨</summary>
        public Currency UnitOfCurrency { get; private set; } = Currency.JPY;
        /// <summary>プロジェクトの単位時間</summary>
        public TimeType UnitOfTime { get; private set; } = TimeType.Day;
        /// <summary>プロジェクト期間</summary>
        /// <remarks>スケジューラではないため納期ではなく期間を設定する。</remarks>
        public LeadTime Term { get; private set; } = LeadTime.Of();
        /// <summary>予算</summary>
        public Money Badjet { get; private set; } = Money.Of();
        /// <summary>遅延存在金</summary>
        /// <remarks>プロジェクトの納期よりも完了が遅れた場合に単位時間あたりに要求される損害賠償金</remarks>
        public Money LiquidatedDamages { get; private set; } = Money.Of();
        /// <summary>プロジェクト開始</summary>
        public Activity Start { get; } = new Activity();
        /// <summary>プロジェクト完了</summary>
        public Activity End { get; } = new Activity();


        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="unitOfCurrency">通貨単位</param>
        /// <param name="unitOfTime">時間単位</param>
        /// <param name="term">プロジェクト期間</param>
        /// <param name="badjet"></param>
        /// <param name="liquidatedDamages"></param>
        /// <returns></returns>
        public static Project Define(
            Currency unitOfCurrency = Currency.JPY,
            TimeType unitOfTime = TimeType.Day,
            decimal term =0m,
            decimal badjet=0m,
            decimal liquidatedDamages=0m
            )
        {
            var project = new Project()
            {
                UnitOfCurrency = unitOfCurrency,
                UnitOfTime = unitOfTime,
                Term = LeadTime.Of(term),
                Badjet = Money.Of(badjet),
                LiquidatedDamages = Money.Of(liquidatedDamages),                
            };
            project.Start.Precede(project.End);
            project.Activities = new[] { project.Start, project.End };
            return project;
        }


        /// <summary>アクティビティ追加</summary>
        /// <param name="activities">アクティビティ群</param>
        public void AddActivities(params Activity[] activities)
        {
            AddActivities(activities as IEnumerable<Activity>);
        }

        /// <summary>アクティビティ追加</summary>
        /// <param name="activities">アクティビティ群</param>
        public void AddActivities(IEnumerable<Activity> activities)
        {
            Start.Branch(activities.Where(a => a.Parents.Count() == 0));
            End.Merge(activities.Where(a => a.Children.Count() == 0));
            Activities = Activities.Union(activities);
            if (Start.Children.Any(a => a == End)) Start.Remove(End);
            if (End.Parents.Any(a => a == Start)) End.Remove(Start);
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
