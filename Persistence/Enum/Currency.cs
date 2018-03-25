namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>通貨列挙型</summary>
    /// <remarks>ISO 4217を参照のこと</remarks>
    public enum CurrencyType
    {
        /// <summary>通貨なし</summary>
        XXX = 0,
        /// <summary>日本円</summary>
        JPY = 1,
        /// <summary>米ドル</summary>
        USD = 2,
        /// <summary>ユーロ</summary>
        EUR = 3,
    }
}