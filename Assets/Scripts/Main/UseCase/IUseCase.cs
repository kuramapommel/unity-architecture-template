namespace UseCase
{
    /// <summary>
    /// ユースケースであること表す interface
    /// </summary>
    public interface IUseCase<ResultType>
    {
        /// <summary>
        /// ユースケースを実行する
        /// </summary>
        /// <returns>実行結果</returns>
        IApplicationResult<ResultType> Execute();
    }
}