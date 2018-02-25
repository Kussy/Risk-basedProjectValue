namespace Kussy.Analysis.Project.Core
{
    /// <summary>見積能力を与えるインターフェース</summary>
    interface IEstimatable
    {
        /// <summary>作業量</summary>
        WorkLoad WorkLoad { get; }
        /// <summary>固定時間</summary>
        LeadTime FixTime { get; }
        /// <summary>収入</summary>
        Income Income { get; }
        /// <summary>支出</summary>
        Cost Cost { get; }
        /// <summary>リスク</summary>
        Risk Risk { get; }

        /// <summary>作業量見積</summary>
        /// <param name="workLoad">作業量</param>
        void Estimate(WorkLoad workLoad);
        /// <summary>固定時間見積</summary>
        /// <param name="leadTime">固定時間</param>
        void Estimate(LeadTime leadTime);
        /// <summary>収入見積</summary>
        /// <param name="income">収入</param>
        void Estimate(Income income);
        /// <summary>支出見積</summary>
        /// <param name="cost">支出</param>
        void Estimate(Cost cost);
        /// <summary>リスク見積</summary>
        /// <param name="risk">リスク</param>
        void Estimate(Risk risk);
    }
}
