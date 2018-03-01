namespace Kussy.Analysis.Project.Core
{
    /// <summary>支出</summary>
    public class Cost : Money
    {
        /// <summary>コンストラクタ</summary>
        /// <param name="value">値</param>
        /// <param name="currency">通貨</param>
        public Cost(decimal value, Currency currency) : base(value, currency) { }
    }
}