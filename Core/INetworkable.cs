﻿using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>接続能力を与える</summary>
    public interface INetworkable
    {
        /// <summary>先行群</summary>
        IEnumerable<INetworkable> Parents { get; }
        /// <summary>後続群</summary>
        IEnumerable<INetworkable> Children { get; }
        /// <summary>先祖</summary>
        IEnumerable<INetworkable> Ancestors { get; }
        /// <summary>子孫</summary>
        IEnumerable<INetworkable> Descendants { get; }

        /// <summary>先行する</summary>
        /// <param name="children">後続</param>
        void Precede(params INetworkable[] children);
        /// <summary>先行する</summary>
        /// <param name="children">後続</param>
        void Precede(IEnumerable<INetworkable> children);
        /// <summary>後続する</summary>
        /// <param name="parents">先行</param>
        void Succeed(params INetworkable[] parents);
        /// <summary>後続する</summary>
        /// <param name="parents">先行</param>
        void Succeed(IEnumerable<INetworkable> parents);
        /// <summary>関係を断つ</summary>
        /// <param name="related">関係者</param>
        void Remove(INetworkable related);
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
        /// <summary>トータルフロートを求める</summary>
        /// <returns>トータルフロート</returns>
        LeadTime TotalFloat();
        /// <summary>フリーフロートを求める</summary>
        /// <returns>フリーフロート</returns>
        LeadTime FreeFloat();
        /// <summary>ディペンデントフロートを求める</summary>
        /// <returns>ディペンデントフロート</returns>
        LeadTime DependentFloat();
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
    }
}
