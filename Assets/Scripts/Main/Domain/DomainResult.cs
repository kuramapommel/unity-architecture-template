using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// ドメイン層処理結果を包むための構造体
    /// </summary>
    public readonly struct DomainResult<ResultType>
    {
        /// <summary>
        /// 成功時ファクトリ
        /// </summary>
        /// <returns>成功時実行結果</returns>
        /// <param name="result">Result.</param>
        public static DomainResult<ResultType> Right(ResultType result) => new DomainResult<ResultType>(result, Enumerable.Empty<DomainError>());

        /// <summary>
        /// 失敗時ファクトリ
        /// </summary>
        /// <returns>失敗時実行結果</returns>
        /// <param name="errors">Errors.</param>
        public static DomainResult<ResultType> Left(params DomainError[] errors) => new DomainResult<ResultType>(default, errors);

        /// <summary>
        /// 失敗時ファクトリ
        /// </summary>
        /// <returns>失敗時実行結果</returns>
        /// <param name="errors">Errors.</param>
        public static DomainResult<ResultType> Left(IEnumerable<DomainError> errors) => new DomainResult<ResultType>(default, errors);

        /// <summary>
        /// ドメイン層で発生したエラー情報
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<DomainError> Errors { get; }

        /// <summary>
        /// ドメイン層の処理結果
        /// </summary>
        /// <value>The result.</value>
        public ResultType Result { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">ドメイン層の処理結果</param>
        /// <param name="errors">ドメイン層で発生したエラー情報</param>
        private DomainResult(ResultType result, params DomainError[] errors) => (Errors, Result) = (errors, result);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">ドメイン層の処理結果</param>
        /// <param name="errors">ドメイン層で発生したエラー情報</param>
        private DomainResult(ResultType result, IEnumerable<DomainError> errors) => (Errors, Result) = (errors, result);
    }
}
