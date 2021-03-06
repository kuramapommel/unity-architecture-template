using System;
using Domain.Exceptions;

namespace Domain
{
    /// <summary>
    /// エラーレベル
    /// </summary>
    public enum ErrorLevel
    {
        ERROR,
        WARNING,
        IGNORABLE
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
    /// 想定外の例外
    /// </summary>
    public readonly struct UnexpectedError : IDomainError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        public string Message => "想定外の例外が発生しました";

        /// <summary>
        /// エラーレベル
        /// </summary>
        /// <value>The level.</value>
        public ErrorLevel Level => ErrorLevel.ERROR;

        /// <summary>
        /// 例外本体
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="exception">Exception.</param>
        public UnexpectedError(Exception exception = null) => Exception = exception ?? new UnexpectedException();
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
        public string Message => "Value Object のインスタンス化に失敗しました";

        /// <summary>
        /// エラーレベル
        /// </summary>
        /// <value>The level.</value>
        public ErrorLevel Level => ErrorLevel.ERROR;

        /// <summary>
        /// 例外本体
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="exception">Exception.</param>
        public ValueObjectCreatedError(Exception exception) => Exception = exception;
    }

    /// <summary>
    /// バリデーションエラー
    /// </summary>
    public readonly struct ValidationError : IDomainError
    {
        /// <summary>
        /// 例外文言
        /// </summary>
        /// <value>The message.</value>
        public string Message => Exception.Message;

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
        public ValidationError(ValidationException exception, ErrorLevel level) => (Exception, Level) = (exception, level);
    }

    public readonly struct NotFoundError : IDomainError
    {
        public string Message { get; }

        public ErrorLevel Level { get; }

        public Exception Exception { get; }

        public NotFoundError(string message, Exception exception, ErrorLevel level) => (Message, Level, Exception) = (message, level, exception);

    }
}