namespace Kussy.Analysis.Project.Core
{
    /// <summary>進捗状態</summary>
    public enum State
    {
        /// <summary>未定義</summary>
        Unknown = 0,
        /// <summary>未着手</summary>
        ToDo = 1,
        /// <summary>着手中</summary>
        Doing = 2,
        /// <summary>完了</summary>
        Done = 3,
    }
}