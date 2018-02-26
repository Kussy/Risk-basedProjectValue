﻿namespace Kussy.Analysis.Project.Core
{
    /// <summary>評価能力を与える</summary>
    interface IEvaluable
    {
        /// <summary>貢献価値を求める</summary>
        /// <returns>貢献価値</returns>
        Currency ContributedValue();
        /// <summary>将来キャッシュフローを求める</summary>
        /// <returns>将来キャッシュフロー</returns>
        Currency ExpectedCachFlow();
    }
}