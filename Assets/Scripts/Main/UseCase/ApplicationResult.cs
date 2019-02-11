using System.Collections.Generic;
using System.Linq;

namespace UseCase
{
    /// <summary>
    /// アプリケーション層の実行結果
    /// </summary>
    public readonly struct ApplicationResult<ResultType>
    {
        /// <summary>
        /// 成功時ファクトリ
        /// </summary>
        /// <returns>成功時実行結果</returns>
        /// <param name="result">Result.</param>
        public static ApplicationResult<ResultType> Right(ResultType result) => new ApplicationResult<ResultType>(result, Enumerable.Empty<ApplicationError>());

        /// <summary>
        /// 失敗時ファクトリ
        /// </summary>
        /// <returns>失敗時実行結果</returns>
        /// <param name="errors">Errors.</param>
        public static ApplicationResult<ResultType> Left(params ApplicationError[] errors) => new ApplicationResult<ResultType>(default, errors);

        /// <summary>
        /// 失敗時ファクトリ
        /// </summary>
        /// <returns>失敗時実行結果</returns>
        /// <param name="errors">Errors.</param>
        public static ApplicationResult<ResultType> Left(IEnumerable<ApplicationError> errors) => new ApplicationResult<ResultType>(default, errors);

        /// <summary>
        /// アプリケーション層で発生した例外
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<ApplicationError> Errors { get; }

        /// <summary>
        /// アプリケーション層の実行結果
        /// </summary>
        /// <value>The result.</value>
        public ResultType Result { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">Result.</param>
        /// <param name="errors">Errors.</param>
        private ApplicationResult(ResultType result, params ApplicationError[] errors) => (Errors, Result) = (errors, result);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">Result.</param>
        /// <param name="errors">Errors.</param>
        private ApplicationResult(ResultType result, IEnumerable<ApplicationError> errors) => (Errors, Result) = (errors, result);
    }
}