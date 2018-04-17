using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>作業を表現するクラス</summary>
    /// <remarks>非常に多くの能力を持つために肥大化した場合を検討しておく</remarks>
    public class Activity : IProgressable
        , IEstimatable
        , IAssignable
        , INetworkable
        , IEvaluable
    {
        /// <summary>GUID</summary>
        public Guid Guid { get; } = Guid.NewGuid();
        /// <summary>ユーザー定義ID</summary>
        public string Id { get; private set; } = string.Empty;
        /// <summary>名称</summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>進捗状態</summary>
        public State State { get; private set; } = State.ToDo;
        /// <summary>作業量</summary>
        public WorkLoad WorkLoad { get; private set; } = WorkLoad.Of();
        /// <summary>固定時間</summary>
        public LeadTime FixTime { get; private set; } = LeadTime.Of();
        /// <summary>収入</summary>
        public Income Income { get; private set; } = Income.Of();
        /// <summary>支出</summary>
        public Cost ExternalCost { get; private set; } = Cost.Of();
        /// <summary>リスク</summary>
        public Risk Risk { get; private set; } = Risk.Of();
        /// <summary>資源</summary>
        public IEnumerable<Resource> Resources { get; private set; } = Enumerable.Empty<Resource>();
        /// <summary>先行群</summary>
        public IEnumerable<INetworkable> Parents { get; private set; } = Enumerable.Empty<INetworkable>();
        /// <summary>後続群</summary>
        public IEnumerable<INetworkable> Children { get; private set; } = Enumerable.Empty<INetworkable>();
        /// <summary>先祖</summary>
        public IEnumerable<INetworkable> Ancestors
        {
            get
            {
                IEnumerable<INetworkable> ancestors(IEnumerable<INetworkable> networkables)
                {
                    if (networkables.IsEmpty()) return networkables;
                    return networkables.Union(ancestors(networkables.SelectMany(a => a.Parents)));
                }
                return ancestors(Parents);
            }
        }
        /// <summary>子孫</summary>
        public IEnumerable<INetworkable> Descendants
        {
            get
            {
                IEnumerable<INetworkable> descendants(IEnumerable<INetworkable> networkables)
                {
                    if (networkables.IsEmpty()) return networkables;
                    return networkables.Union(descendants(networkables.SelectMany(a => a.Children)));
                }
                return descendants(Children);
            }
        }

        /// <summary>アクティビティを定義する</summary>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="externalCost">支出</param>
        /// <param name="income">収入</param>
        /// <param name="fixTime">固定時間</param>
        /// <param name="workLoad">作業量</param>
        /// <param name="failRate">失敗確率</param>
        /// <param name="reworkRate">リワーク確率</param>
        /// <param name="costOverRate">コスト超過確率</param>
        /// <returns></returns>
        public static Activity Define(
            string id = null,
            string name = null,
            decimal externalCost = 0m,
            decimal income = 0m,
            decimal fixTime = 0m,
            decimal workLoad = 0m,
            decimal failRate = 0m,
            decimal reworkRate = 0m,
            decimal costOverRate = 0m)
        {
            Contract.Requires(!id.IsNullOrEmpty());
            Contract.Requires(!name.IsNullOrEmpty());

            var activity = new Activity()
            {
                Id = id,
                Name = name,
            };
            activity.Estimate(Cost.Of(externalCost));
            activity.Estimate(Income.Of(income));
            activity.Estimate(LeadTime.Of(fixTime));
            activity.Estimate(WorkLoad.Of(workLoad));
            activity.Estimate(Risk.Of(failRate: failRate, reworkRate: reworkRate, costOverRate: costOverRate));
            return activity;
        }

        /// <summary>資源群を割当てる</summary>
        /// <param name="resources">資源群</param>
        public void Assign(params Resource[] resources)
        {
            Assign(resources as IEnumerable<Resource>);
        }

        /// <summary>資源群を割当てる</summary>
        /// <param name="resources">資源群</param>
        public void Assign(IEnumerable<Resource> resources)
        {
            Resources = Resources.Concat(resources);
        }

        /// <summary>資源割当を解除する</summary>
        public void UnAssign()
        {
            Resources = Enumerable.Empty<Resource>();
        }

        /// <summary>作業量見積</summary>
        /// <param name="workLoad">作業量</param>
        public void Estimate(WorkLoad workLoad)
        {
            WorkLoad = workLoad;
        }

        /// <summary>固定時間見積</summary>
        /// <param name="fixTime">固定時間</param>
        public void Estimate(LeadTime fixTime)
        {
            FixTime = fixTime;
        }

        /// <summary>収入見積</summary>
        /// <param name="income">収入</param>
        public void Estimate(Income income)
        {
            Income = income;
        }

        /// <summary>支出見積</summary>
        /// <param name="externalCost">支出</param>
        public void Estimate(Cost externalCost)
        {
            ExternalCost = externalCost;
        }

        /// <summary>リスク見積</summary>
        /// <param name="risk">リスク</param>
        public void Estimate(Risk risk)
        {
            Risk = risk;
        }

        /// <summary>進捗を確認する</summary>
        /// <param name="state">進捗状態</param>
        public void Progress(State state)
        {
            State = state;
        }

        /// <summary>先行する</summary>
        /// <param name="children">後続</param>
        /// <remarks>コーディング補助のためのオーバーライド</remarks>
        public void Precede(params INetworkable[] children)
        {
            Precede(children as IEnumerable<INetworkable>);
        }

        /// <summary>先行する</summary>
        /// <param name="children">後続</param>
        public void Precede(IEnumerable< INetworkable> children)
        {
            Contract.Requires(!children.Contains(this));
            Contract.Requires(!children.SelectMany(a => a.Descendants).Contains(this));

            if (children.IsNullOrEmpty()) return;
            if (children.All(a => Children.Contains(a))) return;
            Children = Children.Union(children);
            if (children.All(a => a.Parents.Contains(this))) return;
            foreach (var child in children) child.Succeed(this);
        }

        /// <summary>後続する</summary>
        /// <param name="parents">先行</param>
        /// <remarks>コーディング補助のためのオーバーライド</remarks>
        public void Succeed(params INetworkable[] parents)
        {
            Succeed(parents as IEnumerable<INetworkable>);
        }

        /// <summary>後続する</summary>
        /// <param name="parents">先行</param>
        public void Succeed(IEnumerable< INetworkable> parents)
        {
            Contract.Requires(!parents.Contains(this));
            Contract.Requires(!parents.SelectMany(a => a.Ancestors).Contains(this));

            if (parents.IsNullOrEmpty()) return;
            if (parents.All(a => Parents.Contains(a))) return;
            Parents = Parents.Union(parents);
            if (parents.All(a => a.Parents.Contains(this))) return;
            foreach (var parent in parents) parent.Precede(this);
        }

        /// <summary>関係を断つ</summary>
        /// <param name="related">関係者</param>
        public void Remove(INetworkable related)
        {
            if (!Parents.Contains(related) && !Children.Contains(related)) return;
            Parents = Parents.Except(new[] { related });
            Children = Children.Except(new[] { related });
            related.Remove(this);
        }

        /// <summary>貢献価値を求める</summary>
        /// <returns>貢献価値</returns>
        public Money ContributedValue()
        {
            return Risk.FailRate * (Income + ExpectedFutureCachFlow());
        }

        /// <summary>原始キャッシュフローを求める</summary>
        /// <returns>原始キャッシュフロー</returns>
        public Money PrimevalCashFlow()
        {
            return Income - ExternalCost;
        }

        /// <summary>キャッシュフロー期待値を求める</summary>
        /// <returns>キャッシュフロー期待値</returns>
        public Money ExpectedCachFlow()
        {
            return ArrivalProbability() * Risk.SuccessRate * Income - ArrivalProbability() * ExternalCost;
        }

        /// <summary>将来キャッシュフローを求める</summary>
        /// <returns>将来キャッシュフロー</returns>
        public Money ExpectedFutureCachFlow()
        {
            // 期待キャッシュフロー合計
            // アクティビティ群それぞれのキャッシュフロー期待値と後続群の期待キャッシュフロー合計を取得する
            Money expectedFutureCachFlow(IEnumerable<INetworkable> activities)
            {
                if (activities.IsEmpty()) return Money.Of();
                var sum = Money.Of();
                foreach (Activity actibity in activities)
                {
                    sum +=
                    actibity.Risk.SuccessRate * actibity.Income -
                    actibity.ExternalCost +
                    actibity.Risk.SuccessRate * expectedFutureCachFlow(actibity.Children);
                }
                return sum;
            }
            return expectedFutureCachFlow(Children);
        }

        /// <summary>到達確率を求める</summary>
        /// <returns>到達確率</returns>
        /// <remarks>先行工程が前完了していないと開始できないため、並列でも余事象ではない。</remarks>
        public decimal ArrivalProbability()
        {
            var accumulatedProbability = 1m;

            if (Parents.IsEmpty()) return accumulatedProbability;

            foreach (Activity parent in Parents)
            {
                accumulatedProbability
                    = parent.State == State.Done
                    ? parent.ArrivalProbability()
                    : parent.ArrivalProbability() * parent.Risk.SuccessRate;
            }
            return accumulatedProbability;
        }

        /// <summary>所要期間を求める</summary>
        /// <returns>所要期間</returns>
        /// <remarks>
        /// 固定時間がある場合は固定時間
        /// 固定時間がない場合は作業量/総資源生産性
        /// </remarks>
        public LeadTime Duration()
        {
            if (FixTime.Value != 0m) return FixTime;
            else if (WorkLoad.Value == 0m) return LeadTime.Of();
            else return WorkLoad / Resources;
        }

        /// <summary>最早着手日を求める</summary>
        /// <returns>最早着手日</returns>
        public LeadTime EarliestStart()
        {
            if (Parents.IsEmpty()) return LeadTime.Of(0m);
            return Parents.Max(a => a.EarliestFinish());
        }

        /// <summary>最早完了日を求める</summary>
        /// <returns>最早完了日</returns>
        public LeadTime EarliestFinish()
        {
            return EarliestStart() + Duration();
        }

        /// <summary>最遅着手日を求める</summary>
        /// <returns>最遅着手日</returns>
        public LeadTime LatestStart()
        {
            return LatestFinish() - Duration();
        }

        /// <summary>最遅完了日を求める</summary>
        /// <returns>最遅完了日</returns>
        public LeadTime LatestFinish()
        {
            if (Children.IsEmpty()) return EarliestFinish();
            return Children.Min(a => a.LatestStart());
        }

        /// <summary>トータルフロートを求める</summary>
        /// <returns>トータルフロート</returns>
        public LeadTime TotalFloat()
        {
            return LatestStart() - EarliestStart();
        }

        /// <summary>フリーフロートを求める</summary>
        /// <returns>フリーフロート</returns>
        public LeadTime FreeFloat()
        {
            return Children.Min(a=>a.EarliestStart())  - EarliestFinish();
        }

        /// <summary>ディペンデントフロートを求める</summary>
        /// <returns>ディペンデントフロート</returns>
        public LeadTime DependentFloat()
        {
            return TotalFloat() - FreeFloat();
        }

        /// <summary>DRAGを求める</summary>
        /// <returns>DRAG</returns>
        /// <remarks>
        /// アクティビティの所要時間をゼロにした場合の完了時間の変化
        /// 非クリティカル・パスの場合はゼロ
        /// クリティカル・パスの場合は並列アクティビティの有無で変化する
        /// </remarks>
        public LeadTime Drag()
        {
            if (Children.IsNullOrEmpty()) return Duration();
            var originalEndTime = Descendants.Where(d => d.Children.IsEmpty()).First().EarliestFinish().Value;
            var originalFixTime = FixTime;
            var originalWorkload = WorkLoad;
            Estimate(LeadTime.Of());
            Estimate(WorkLoad.Of());
            var shortenedEndTime = Descendants.Where(d => d.Children.IsEmpty()).First().EarliestFinish().Value;
            var drag = originalEndTime - shortenedEndTime;
            Estimate(originalFixTime);
            Estimate(originalWorkload);
            return LeadTime.Of(drag);
        }

        /// <summary>DRAGコストを求める</summary>
        /// <param name="liquidatedDamages">遅延存在賠償金</param>
        /// <returns>DRAGコスト</returns>
        public Money DragCost(Money liquidatedDamages)
        {
            return Drag().Value * liquidatedDamages;
        }

        /// <summary>クリティカル・パスに乗っているかを判定する</summary>
        /// <returns>true:クリティカル・パス/false:非クリティカル・パス</returns>
        public bool IsInCriticalPath()
        {
            return TotalFloat().Value == 0m;
        }

        /// <summary>本質的コストを求める</summary>
        /// <param name="liquidatedDamages">遅延損害金</param>
        /// <returns>本質的コスト</returns>
        public Money IntrinsicCost(Money liquidatedDamages)
        {
            return ExternalCost + DragCost(liquidatedDamages);
        }
    }
}