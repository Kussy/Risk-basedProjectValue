using System.Collections.Generic;

namespace Kussy.Analysis.Project.Core
{
    /// <summary>リスク</summary>
    public class Risk:ValueObject
    {
        /// <summary>失敗確率</summary>
        public decimal FailRate { get; private set; }
        /// <summary>リワーク確率</summary>
        public decimal ReworkRate { get; private set; }
        /// <summary>コスト超過確率</summary>
        public decimal CostOverRate { get; private set; }

        /// <summary>プライベートコンストラクタ</summary>
        private Risk() { }
        /// <summary>コンストラクタ</summary>
        /// <param name="failRate">失敗確率</param>
        /// <param name="reworkRate">リワーク確率</param>
        /// <param name="costOverRate">コスト超過確率</param>
        public Risk(decimal failRate, decimal reworkRate, decimal costOverRate)
        {
            FailRate = failRate;
            ReworkRate = reworkRate;
            CostOverRate = costOverRate;
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