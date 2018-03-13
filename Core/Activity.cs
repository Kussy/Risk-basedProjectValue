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
            return Risk.FailRate * (Income + ExpectedFutureCachFlow());
        }

        /// <summary>原始キャッシュフローを求める</summary>
        /// <returns>原始キャッシュフロー</returns>
        public Money PrimevalCashFlow()
        {
            return Income - DirectCost;
        }

        /// <summary>キャッシュフロー期待値を求める</summary>
        /// <returns>キャッシュフロー期待値</returns>
        public Money ExpectedCachFlow()
        {
            return ArrivalProbability() * Risk.SuccessRate * Income - ArrivalProbability() * DirectCost;
        }

        /// <summary>将来キャッシュフローを求める</summary>
        /// <returns>将来キャッシュフロー</returns>
        public Money ExpectedFutureCachFlow()
        {
            // 期待キャッシュフロー合計
            // 再帰的に呼び出すため宣言を分離
            Func<IEnumerable<INetworkable>, Money> expectedFutureCachFlow = null;
            // 引数に並列アクティビティを受け取り、それぞれのキャッシュフロー期待値と後続群の期待キャッシュフロー合計を取得する
            expectedFutureCachFlow = actibities =>
            {
                if (actibities.Count() == 0) return Money.Of();
                var sum = Money.Of();
                foreach (Activity actibity in actibities)
                {
                    sum +=
                    actibity.Risk.SuccessRate * actibity.Income -
                    actibity.DirectCost +
                    actibity.Risk.SuccessRate * expectedFutureCachFlow(actibity.Children);
                }
                return sum;
            };
            return expectedFutureCachFlow(Children);
        }

        /// <summary>到達確率を求める</summary>
        /// <returns>到達確率</returns>
        /// <remarks>先行工程が前完了していないと開始できないため、並列でも余事象ではない。</remarks>
        public decimal ArrivalProbability()
        {
            var accumulatedProbability = 1m;

            if (Parents.Count() == 0) return accumulatedProbability;

            foreach (Activity parent in Parents)
            {
                accumulatedProbability
                    = parent.State == State.Done
                    ? parent.ArrivalProbability()
                    : parent.ArrivalProbability() * parent.Risk.SuccessRate;
            }
            return accumulatedProbability;
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

        /// <summary>DRAGを求める</summary>
        /// <returns>DRAG</returns>
        /// <remarks>
        /// 非クリティカル・パスならば DRAG＝0
        /// クリティカル・パスかつ他に並行作業がないならばDRAG＝所要時間
        /// クリティカル・パスかつ並行作業があるならばDRAG＝並行作業系列のFloat
        /// 所要時間＜並行作業のFloatならばDRAG＝所要時間
        /// </remarks>
        public LeadTime Drag()
        {
            var leadTime = FixTime.Value != 0
                ? FixTime
                : WorkLoad / Resources;

            if (!IsInCriticalPath())
            {
                return LeadTime.Of();
            }
            else if (!ExistsParallelActivity())
            {
                return leadTime;
            }
            else
            {
                var parallels = Parents.SelectMany(a => a.Children).Where(a => a != this);
                var minFloat = parallels.Min(a => (a as Activity).Float());
                return leadTime.CompareTo(minFloat) == -1
                    ? leadTime
                    : minFloat;
            }
        }

        /// <summary>クリティカル・パスに乗っているかを判定する</summary>
        /// <returns>true:クリティカル・パス/false:非クリティカル・パス</returns>
        public bool IsInCriticalPath()
        {
            return Float().Value == 0m;
        }

        /// <summary>並列アクティビティが存在するかを判定する</summary>
        /// <returns>true:並列あり/false:並列なし</returns>
        public bool ExistsParallelActivity()
        {
            if (Parents.Count() == 0) return false;
            return Parents.SelectMany(a => a.Children).Distinct().Count() > 1;
        }
    }
}