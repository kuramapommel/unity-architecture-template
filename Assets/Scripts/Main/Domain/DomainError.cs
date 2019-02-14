using System;

namespace Domain
{
    public enum ErrorLevel
    {
        ERROR,
        WARNING,
        IGNORE
    }

    /// <summary>
    /// ドメイン層で発生した例外情報 interface
    /// </summary>
    public interface IDomainError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        string Message { get; }

        /// <summary>
        /// エラーレベル
        /// </summary>
        /// <value>The level.</value>
        ErrorLevel Level { get; }

        /// <summary>
        /// 例外本体
        /// </summary>
        /// <value>The exception.</value>
        Exception Exception { get; }
    }

    /// <summary>
    ///  Value Object 生成エラー
    /// </summary>
    public readonly struct ValueObjectCreatedError : IDomainError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        /// エラーレベル
        /// </summary>
        /// <value>The level.</value>
        public ErrorLevel Level { get; }

        /// <summary>
        /// 例外本体
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="exception">Exception.</param>
        public ValueObjectCreatedError(Exception exception) => (Message, Level, Exception) = ("Value Object のインスタンス化に失敗しました", ErrorLevel.ERROR, exception);
    }
}

namespace Domain.Exceptions
{
    public sealed class ValidateException : Exception
    {

    }
}
