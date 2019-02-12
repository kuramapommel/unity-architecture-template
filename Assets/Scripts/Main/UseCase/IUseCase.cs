namespace UseCase
{
    /// <summary>
    /// ユースケースであること表す interface
    /// </summary>
    public interface IUseCase<out ResultType, ProtocolDomainType>
    {
        /// <summary>
        /// ユースケースを実行する
        /// </summary>
        /// <returns>実行結果</returns>
        /// UniTask が使える環境であれば async メソッドにする
        IApplicationResult<ResultType> Execute(IUseCaseProtocol<ProtocolDomainType> protocol);
    }

    /// <summary>
    /// ユースケースを実行するためのプロトコルを表す interface
    /// </summary>
    public interface IUseCaseProtocol<Protocol>
    {
        /// <summary>
        /// アプリケーション層以下で使用する型に変換する
        /// </summary>
        /// <returns>The use case protocol.</returns>
        Protocol ToUseCaseProtocol();
    }
}