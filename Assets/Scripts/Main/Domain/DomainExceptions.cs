using System;

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