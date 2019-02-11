using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// ドメイン層の処理結果を表す interface
    /// </summary>
    public interface IDomainResult
    {
    }

    /// <summary>
    /// ドメイン層の処理成功を表す interface
    /// </summary>
    public interface Success<out ResultType> : IDomainResult
    {
        /// <summary>
        /// 実行結果
        /// </summary>
        /// <value>The result.</value>
        ResultType Result { get; }
    }

    /// <summary>
    /// ドメイン層の処理失敗を表す interface
    /// </summary>
    public interface Failure : IDomainResult
    {
        /// <summary>
        /// 失敗内容
        /// </summary>
        /// <value>The errors.</value>
        IEnumerable<DomainError> Errors { get; }
    }

    /// <summary>
    /// ドメイン層の処理結果
    /// </summary>
    public static class DomainResult
    {
        /// <summary>
        /// 成功結果生成ファクトリ
        /// </summary>
        /// <returns>The success.</returns>
        /// <param name="result">Result.</param>
        /// <typeparam name="ResultType">The 1st type parameter.</typeparam>
        public static IDomainResult Success<ResultType>(ResultType result) => new SuccessImpl<ResultType>(result);

        /// <summary>
        /// 失敗結果生成ファクトリ
        /// </summary>
        /// <returns>The failure.</returns>
        /// <param name="errors">Errors.</param>
        public static IDomainResult Failure(params DomainError[] errors) => new FailureImpl(errors);

        /// <summary>
        /// 失敗結果生成ファクトリ
        /// </summary>
        /// <returns>The failure.</returns>
        /// <param name="errors">Errors.</param>
        public static IDomainResult Failure(IEnumerable<DomainError> errors) => new FailureImpl(errors);

        /// <summary>
        /// 成功結果具象実装構造体
        /// </summary>
        private sealed class SuccessImpl<ResultType> : Success<ResultType>
        {
            /// <summary>
            /// 実行結果
            /// </summary>
            /// <value>The result.</value>
            public ResultType Result { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="resutl">Resutl.</param>
            public SuccessImpl(ResultType resutl) => Result = resutl;
        }

        /// <summary>
        /// 失敗結果具象実装構造体
        /// </summary>
        private sealed class FailureImpl : Failure
        {
            /// <summary>
            /// 失敗内容
            /// </summary>
            /// <value>The errors.</value>
            public IEnumerable<DomainError> Errors { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="errors">Errors.</param>
            public FailureImpl(IEnumerable<DomainError> errors) => Errors = errors;
        }
    }
}
