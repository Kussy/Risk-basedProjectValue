namespace Kussy.Analysis.Project.Core
{
    /// <summary>進捗能力を与える</summary>
    interface IProgressable
    {
        /// <summary>状態</summary>
        State State { get; }
        /// <summary>進捗を確認する</summary>
        /// <param name="state">進捗状態</param>
        void Progress(State state);
    }
}
