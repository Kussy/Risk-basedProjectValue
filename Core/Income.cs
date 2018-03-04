namespace Kussy.Analysis.Project.Core
{
    /// <summary>収入</summary>
    public class Income : Money
    {
        /// <summary>コンストラクタ</summary>
        /// <param name="value">値</param>
        /// <param name="currency">通貨</param>
        public Income(decimal value, Currency currency) : base(value, currency) { }
    }
}