﻿namespace Kussy.Analysis.Project.Core
{
    /// <summary>計画能力を与える</summary>
    interface IPlannable
    {
        /// <summary>所要期間を求める</summary>
        /// <returns>所要期間</returns>
        LeadTime Duration();
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
        /// <summary>DRAGを求める</summary>
        /// <returns>DRAG</returns>
        LeadTime Drag();
        /// <summary>DRAGコストを求める</summary>
        /// <param name="liquidatedDamages">遅延存在賠償金</param>
        /// <returns>DRAGコスト</returns>
        Money DragCost(Money liquidatedDamages);
        /// <summary>クリティカル・パスに乗っているかを判定する</summary>
        /// <returns>true:クリティカル・パス/false:非クリティカル・パス</returns>
        bool IsInCriticalPath();
        /// <summary>並列アクティビティが存在するかを判定する</summary>
        /// <returns>true:並列あり/false:並列なし</returns>
        bool ExistsParallelActivity();
    }
}