using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>リスク</summary>
    public class Risk : ValueObject
    {
        /// <summary>失敗確率</summary>
        public decimal FailRate { get; private set; } = 0m;
        /// <summary>リワーク確率</summary>
        public decimal ReworkRate { get; private set; } = 0m;
        /// <summary>コスト超過確率</summary>
        public decimal CostOverRate { get; private set; } = 0m;

        /// <summary>プライベートコンストラクタ</summary>
        private Risk() { }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <returns>インスタンス初期値</returns>
        public static Risk Of()
        {
            return new Risk();
        }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="failRate">失敗確率</param>
        /// <param name="reworkRate">リワーク確率</param>
        /// <param name="costOverRate">コスト超過確率</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public static Risk Of(decimal failRate, decimal reworkRate, decimal costOverRate)
        {
            return new Risk()
            {
                FailRate = failRate,
                ReworkRate = reworkRate,
                CostOverRate = costOverRate,
            };
        }

        /// <summary>プロパティを反復処理する</summary>
        /// <returns>要素列挙</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FailRate;
            yield return ReworkRate;
            yield return CostOverRate;
        }
    }
}