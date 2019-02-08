using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// ドメイン層処理結果を包むための構造体
    /// </summary>
    public readonly struct DomainResult<ResultType>
    {
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
        public DomainResult(ResultType result, params DomainError[] errors) => (Errors, Result) = (errors, result);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">ドメイン層の処理結果</param>
        /// <param name="errors">ドメイン層で発生したエラー情報</param>
        public DomainResult(ResultType result, IEnumerable<DomainError> errors) => (Errors, Result) = (errors, result);
    }
}
