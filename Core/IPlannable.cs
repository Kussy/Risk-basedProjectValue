namespace Kussy.Analysis.Project.Core
{
    /// <summary>計画能力を与える</summary>
    interface IPlannable
    {
        /// <summary>最早着手日を求める</summary>
        /// <returns>最早着手日</returns>
        LeadTime EarliestStart();
        /// <summary>最早完了日を求める</summary>
        /// <returns>最早完了日</returns>
        LeadTime EarliestFinish();
        /// <summary>最遅着手日を求める</summary>
        /// <returns>最遅着手日</returns>
        LeadTime LatestStart();
        /// <summary>最遅完了日を求める</summary>
        /// <returns>最遅完了日</returns>
        LeadTime LatestFinish();
        /// <summary>フロートを求める</summary>
        /// <returns>フロート</returns>
        LeadTime Float();
    }
}