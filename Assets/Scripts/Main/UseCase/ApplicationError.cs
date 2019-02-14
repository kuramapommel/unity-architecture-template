using Domain;
using static Domain.ErrorLevel;

namespace UseCase
{
    /// <summary>
    /// アプリケーション層で発生した例外を包むための構造体
    /// </summary>
    public readonly struct ApplicationError
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="domainError">Domain error.</param>
        public ApplicationError(IDomainError domainError) { }
    }

    /// <summary>
    /// ドメイン層エラーの拡張メソッドクラス
    /// </summary>
    public static class DomainErrorExt
    {
        /// <summary>
        /// アプリケーション層エラーに変換する
        /// </summary>
        /// <returns>The application error.</returns>
        /// <param name="domainError">Domain error.</param>
        public static IApplicationError ToApplicationError(this IDomainError domainError)
        {
            switch (domainError.Level)
            {
                case ERROR:
                    return new Error(domainError);
                case WARNING:
                    return new Warning(domainError);
                case IGNORED:
                    return new Ignored(domainError);
            }

            return new Error(domainError);
        }
    }

    /// <summary>
    /// アプリケーション層エラー
    /// </summary>
    public interface IApplicationError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        string Message { get; }

        /// <summary>
        /// ドメイン層の例外
        /// </summary>
        /// <value>The domain error.</value>
        IDomainError DomainError { get; }
    }

    /// <summary>
    /// 復旧不可例外
    /// </summary>
    public sealed class Error : IApplicationError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        public string Message => DomainError.Message;

        /// <summary>
        /// ドメイン層の例外
        /// </summary>
        /// <value>The domain error.</value>
        public IDomainError DomainError { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="domainError">Domain error.</param>
        public Error(IDomainError domainError) => DomainError = domainError;
    }

    /// <summary>
    /// 警告レベルの例外（ユーザ操作による復旧可能）
    /// </summary>
    public sealed class Warning : IApplicationError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        public string Message => DomainError.Message;

        /// <summary>
        /// ドメイン層の例外
        /// </summary>
        /// <value>The domain error.</value>
        public IDomainError DomainError { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="domainError">Domain error.</param>
        public Warning(IDomainError domainError) => DomainError = domainError;
    }

    /// <summary>
    /// 自己復旧可能例外
    /// </summary>
    public sealed class Ignored : IApplicationError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        public string Message => DomainError.Message;

        /// <summary>
        /// ドメイン層の例外
        /// </summary>
        /// <value>The domain error.</value>
        public IDomainError DomainError { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="domainError">Domain error.</param>
        public Ignored(IDomainError domainError) => DomainError = domainError;
    }
}