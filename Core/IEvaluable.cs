namespace Kussy.Analysis.Project.Core
{
    /// <summary>評価能力を与える</summary>
    interface IEvaluable
    {
        /// <summary>貢献価値を求める</summary>
        /// <returns>貢献価値</returns>
        Money ContributedValue();
        /// <summary>原始キャッシュフローを求める</summary>
        /// <returns>原始キャッシュフロー</returns>
        Money PrimevalCashFlow();
        /// <summary>キャッシュフロー期待値を求める</summary>
        /// <returns>キャッシュフロー期待値</returns>
        Money ExpectedCachFlow();
        /// <summary>将来キャッシュフロー期待値を求める</summary>
        /// <returns>将来キャッシュフロー期待値</returns>
        Money ExpectedFutureCachFlow();
        /// <summary>到達確率を求める</summary>
        /// <returns>到達確率</returns>
        decimal ArrivalProbability();
    }
}