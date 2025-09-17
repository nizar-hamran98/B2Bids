namespace SharedKernel
{
    /// <summary>
    /// Unified action result for internal action that deliver a certain logic
    /// </summary>
    /// <typeparam name="T">Action response type</typeparam>
    public interface IBaseAction<T>
    {
        T Response { get; }
        Task ExecuteAsync();
    }

    /// <summary>
    /// Unified action result for internal action that deliver a certain logic
    /// </summary>
    public interface IBaseAction
    {
        void Execute();
    }
}
