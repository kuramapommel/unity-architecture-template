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
        IGNORED
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
}

namespace Domain.Exceptions
{
    /// <summary>
    /// ドメイン層の例外
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DomainException() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">Message.</param>
        public DomainException(string message) : base(message) { }
    }

    /// <summary>
    /// 検証例外
    /// </summary>
    public sealed class ValidationException : DomainException
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationException() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">Message.</param>
        public ValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// 想定外の例外
    /// </summary>
    public sealed class UnexpectedException : DomainException
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnexpectedException() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">Message.</param>
        public UnexpectedException(string message) : base(message) { }
    }
}
