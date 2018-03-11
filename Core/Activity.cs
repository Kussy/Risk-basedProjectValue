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
        , IPlannable
    {
        /// <summary>進捗状態</summary>
        public State State { get; private set; } = State.ToDo;
        /// <summary>作業量</summary>
        public WorkLoad WorkLoad { get; private set; } = WorkLoad.Of();
        /// <summary>固定時間</summary>
        public LeadTime FixTime { get; private set; } = LeadTime.Of();
        /// <summary>収入</summary>
        public Income Income { get; private set; } = Income.Of();
        /// <summary>支出</summary>
        public Cost DirectCost { get; private set; } = Cost.Of();
        /// <summary>リスク</summary>
        public Risk Risk { get; private set; } = Risk.Of();
        /// <summary>資源</summary>
        public IEnumerable<Resource> Resources { get; private set; } = Enumerable.Empty<Resource>();
        /// <summary>先行群</summary>
        public IEnumerable<INetworkable> Parents { get; private set; } = Enumerable.Empty<INetworkable>();
        /// <summary>後続群</summary>
        public IEnumerable<INetworkable> Children { get; private set; } = Enumerable.Empty<INetworkable>();

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
        /// <param name="directCost">支出</param>
        public void Estimate(Cost directCost)
        {
            DirectCost = directCost;
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
        /// <param name="child">後続</param>
        public void Precede(INetworkable child)
        {
            if (Children.Contains(child)) return;
            Children = Children.Union(new[] { child });
            if (child.Parents.Contains(this)) return;
            child.Succeed(this);
        }

        /// <summary>後続する</summary>
        /// <param name="parent">先行</param>
        public void Succeed(INetworkable parent)
        {
            if (Parents.Contains(parent)) return;
            Parents = Parents.Union(new[] { parent });
            if (parent.Parents.Contains(this)) return;
            parent.Precede(this);
        }

        /// <summary>分岐する</summary>
        /// <param name="children">後続群</param>
        public void Branch(IEnumerable<INetworkable> children)
        {
            Children = Children.Union(children);
            foreach (var child in children) child.Succeed(this);
        }

        /// <summary>合流する</summary>
        /// <param name="parents">先行群</param>
        public void Merge(IEnumerable<INetworkable> parents)
        {
            Parents = Parents.Union(parents);
            foreach (var parent in parents) parent.Precede(this);
        }

        /// <summary>貢献価値を求める</summary>
        /// <returns>貢献価値</returns>
        public Money ContributedValue()
        {
            return Risk.FailRate * (Income + ExpectedCachFlow());
        }

        /// <summary>原始キャッシュフローを求める</summary>
        /// <returns>原始キャッシュフロー</returns>
        public Money PrimevalCashFlow()
        {
            return Income - DirectCost;
        }

        /// <summary>将来キャッシュフローを求める</summary>
        /// <returns>将来キャッシュフロー</returns>
        public Money ExpectedCachFlow()
        {
            if (Children.Count() == 0) return Money.Of();

            var value = Children.Sum(c =>
            {
                var child = (c as Activity);
                return (child.Risk.SuccessRate
                * (child.Income + child.ExpectedCachFlow())
                - child.DirectCost).Value;
            });
            return Money.Of(value);
        }

        /// <summary>最早着手日を求める</summary>
        /// <returns>最早着手日</returns>
        public LeadTime EarliestStart()
        {
            if (Parents.Count() == 0) return LeadTime.Of(0m);
            return Parents.Max(a => (a as Activity).EarliestFinish());
        }

        /// <summary>最早完了日を求める</summary>
        /// <returns>最早完了日</returns>
        public LeadTime EarliestFinish()
        {
            return FixTime.Value != 0
                ? EarliestStart() + FixTime
                : EarliestStart() + WorkLoad / Resources;
        }

        /// <summary>最遅着手日を求める</summary>
        /// <returns>最遅着手日</returns>
        public LeadTime LatestStart()
        {
            return FixTime.Value != 0
                ? LatestFinish() - FixTime
                : LatestFinish() - WorkLoad / Resources;
        }

        /// <summary>最遅完了日を求める</summary>
        /// <returns>最遅完了日</returns>
        public LeadTime LatestFinish()
        {
            if (Children.Count() == 0) return EarliestFinish();
            return Children.Min(a => (a as Activity).LatestStart());
        }

        /// <summary>フロートを求める</summary>
        /// <returns>フロート</returns>
        public LeadTime Float()
        {
            return LatestStart() - EarliestStart();
        }
    }
}