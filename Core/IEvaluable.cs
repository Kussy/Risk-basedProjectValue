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
        /// <summary>将来キャッシュフローを求める</summary>
        /// <returns>将来キャッシュフロー</returns>
        Money ExpectedCachFlow();
    }
}