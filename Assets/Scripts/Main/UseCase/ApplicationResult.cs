using System.Collections.Generic;
using System.Collections;

namespace UseCase
{
    /// <summary>
    /// アプリケーション層の処理結果を表す interface
    /// </summary>
    /// foreach や linq で使えるようにするため
    /// IEnumerable を実装する
    public interface IApplicationResult<out ResultType> : IEnumerable, IEnumerable<ResultType>
    {
    }

    /// <summary>
    /// アプリケーション層の処理成功を表す interface
    /// </summary>
    public interface Success<out ResultType> : IApplicationResult<ResultType>
    {
        /// <summary>
        /// 実行結果
        /// </summary>
        /// <value>The result.</value>
        ResultType Result { get; }
    }

    /// <summary>
    /// アプリケーション層の処理失敗を表す interface
    /// </summary>
    public interface Failure<out ResultType> : IApplicationResult<ResultType>
    {
        /// <summary>
        /// 失敗内容
        /// </summary>
        /// <value>The errors.</value>
        IEnumerable<ApplicationError> Errors { get; }
    }

    /// <summary>
    /// アプリケーション層の処理結果
    /// </summary>
    public static class ApplicationResult
    {
        /// <summary>
        /// 成功結果生成ファクトリ
        /// </summary>
        /// <returns>The success.</returns>
        /// <param name="result">Result.</param>
        /// <typeparam name="ResultType">The 1st type parameter.</typeparam>
        public static IApplicationResult<ResultType> Success<ResultType>(ResultType result) => new SuccessImpl<ResultType>(result);

        /// <summary>
        /// 失敗結果生成ファクトリ
        /// </summary>
        /// <returns>The failure.</returns>
        /// <param name="errors">Errors.</param>
        public static IApplicationResult<ResultType> Failure<ResultType>(params ApplicationError[] errors) => new FailureImpl<ResultType>(errors);

        /// <summary>
        /// 失敗結果生成ファクトリ
        /// </summary>
        /// <returns>The failure.</returns>
        /// <param name="errors">Errors.</param>
        public static IApplicationResult<ResultType> Failure<ResultType>(IEnumerable<ApplicationError> errors) => new FailureImpl<ResultType>(errors);

        /// <summary>
        /// 成功結果具象実装構造体
        /// </summary>
        private readonly struct SuccessImpl<ResultType> : Success<ResultType>
        {
            /// <summary>
            /// 実行結果
            /// </summary>
            /// <value>The result.</value>
            public ResultType Result { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="result">Result.</param>
            public SuccessImpl(ResultType result) => Result = result;

            /// <summary>
            /// IEnumerable実装
            /// </summary>
            /// <returns>The enumerator.</returns>
            public IEnumerator<ResultType> GetEnumerator()
            {
                yield return Result;
            }

            /// <summary>
            /// IEnumerable実装
            /// </summary>
            /// <returns>The enumerator.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// 失敗結果具象実装構造体
        /// </summary>
        private readonly struct FailureImpl<ResultType> : Failure<ResultType>
        {
            /// <summary>
            /// 失敗内容
            /// </summary>
            /// <value>The errors.</value>
            public IEnumerable<ApplicationError> Errors { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="errors">Errors.</param>
            public FailureImpl(IEnumerable<ApplicationError> errors) => Errors = errors;

            /// <summary>
            /// IEnumerable実装
            /// </summary>
            /// <returns>The enumerator.</returns>
            public IEnumerator<ResultType> GetEnumerator()
            {
                yield break;
            }

            /// <summary>
            /// IEnumerable実装
            /// </summary>
            /// <returns>The enumerator.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}