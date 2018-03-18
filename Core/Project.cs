﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>プロジェクト</summary>
    public class Project
    {
        /// <summary>GUID</summary>
        public Guid Guid { get; } = Guid.NewGuid();
        /// <summary>ユーザー定義ID</summary>
        public string Id { get; private set; } = string.Empty;
        /// <summary>名称</summary>
        public string Name { get; private set; } = string.Empty;
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
            decimal term = 0m,
            decimal badjet = 0m,
            decimal liquidatedDamages = 0m
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
            Start.Branch(activities.Where(a => a.Parents.IsEmpty()));
            End.Merge(activities.Where(a => a.Children.IsEmpty()));
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
            Contract.Requires(!Activities.IsNullOrEmpty());
            return Start.ExpectedFutureCachFlow();
        }

        /// <summary>完了時点でのRPVを求める</summary>
        /// <returns>完了時点のRPV</returns>
        public Money RPVfinish()
        {
            Contract.Requires(!Activities.IsNullOrEmpty());
            return Activities.Select(a => a.Income - a.DirectCost).Sum();
        }

        /// <summary>現時点でのキャッシュフローを求める</summary>
        /// <returns>現時点のキャッシュフロー</returns>
        public Money RPV()
        {
            Contract.Requires(!Activities.IsNullOrEmpty());
            var accumulatedCF = Activities
                .Where(a => a.State == State.Done)
                .Select(a => a.PrimevalCashFlow())
                .Sum();
            var futureCF = Activities
                .Where(a => a.State != State.Done)
                .Select(a => a.ExpectedCachFlow())
                .Sum();
            return accumulatedCF + futureCF;
        }
    }
}