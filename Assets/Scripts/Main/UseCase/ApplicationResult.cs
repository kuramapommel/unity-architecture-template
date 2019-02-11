using System.Collections.Generic;

namespace UseCase
{
    /// <summary>
    /// アプリケーション層の実行結果
    /// </summary>
    public readonly struct ApplicationResult<ResultType>
    {
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
        public ApplicationResult(ResultType result, params ApplicationError[] errors) => (Errors, Result) = (errors, result);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">Result.</param>
        /// <param name="errors">Errors.</param>
        public ApplicationResult(ResultType result, IEnumerable<ApplicationError> errors) => (Errors, Result) = (errors, result);
    }
}