namespace Kussy.Analysis.Project.Core
{
    /// <summary>収入</summary>
    public class Income : Money
    {
        /// <summary>プライベートコンストラクタ</summary>
        protected Income() : base() { }

        /// <summary>基底クラスからのキャスト用のコンストラクタ</summary>
        /// <param name="money">金銭</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        protected static Income Of(Money money)
        {
            return new Income()
            {
                Value = money.Value,
                Currency = money.Currency,

            };
        }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <returns>インスタンス初期値</returns>
        public new static Income Of()
        {
            return Of((Money.Of()));
        }

        /// <summary>静的ファクトリーメソッド</summary>
        /// <param name="value">値</param>
        /// <param name="currency">通貨</param>
        /// <returns>パラメータと同じ値を持つインスタンス</returns>
        public new static Income Of(decimal value, Currency currency)
        {
            return Of(Money.Of(value, currency));
        }
    }
}