using System;
using Domain;
using Domain.Exceptions;

namespace UseCase
{
    /// <summary>
    /// ユースケースを実行するためのプロトコルを表す interface
    /// </summary>
    public interface IProtocol<UseCaseProtocol>
    {
        /// <summary>
        /// アプリケーション層以下で使用する型に変換する
        /// </summary>
        /// <returns>The use case protocol.</returns>
        UseCaseProtocol ToUseCaseProtocol();
    }

    /// <summary>
    /// ユースケーステンプレート
    /// </summary>
    public abstract class UseCase<ResultType, ProtocolDomainType>
    {
        /// <summary>
        /// ユースケース具象実装
        /// </summary>
        /// <returns>The use case.</returns>
        /// <param name="protocol">Protocol.</param>
        protected abstract IApplicationResult<ResultType> ExecuteImpl(ProtocolDomainType protocol);

        /// <summary>
        /// プロトコルバリデーションエラー時のエラーレベル
        /// </summary>
        /// <value>The validation error level.</value>
        protected abstract ErrorLevel ValidationErrorLevel { get; }

        /// <summary>
        /// ユースケース実行
        /// </summary>
        /// <returns>The execute.</returns>
        /// <param name="protocol">Protocol.</param>
        public IApplicationResult<ResultType> Execute(IProtocol<ProtocolDomainType> protocol)
        {
            /// <summary>
            /// バリデータ
            /// </summary>
            /// <returns>The validated.</returns>
            IDomainResult<ProtocolDomainType> validate()
            {
                try
                {
                    return DomainResult.Success(protocol.ToUseCaseProtocol());
                }
                catch (ValidationException exception)
                {
                    return DomainResult.Failure<ProtocolDomainType>(new ValidationError(exception, ValidationErrorLevel));
                }
            }

            try
            {
                var validated = validate();
                switch (validated)
                {
                    // バリデーション結果がエラーの場合はその情報を詰めて返す
                    case Domain.Failure<ProtocolDomainType> failure:
                        return ApplicationResult.Failure<ResultType>(failure.Reason.ToApplicationError());

                    // バリデーション結果がOKの場合はユースケース実行
                    case Domain.Success<ProtocolDomainType> success:
                        return ExecuteImpl(success.Result);
                }

                // 想定外のエラー
                return ApplicationResult.Unexpected<ResultType>();
            }
            // ユースケース内で拾えなかったすべての例外を想定外の例外として扱う
            catch(Exception exception)
            {
                return ApplicationResult.Unexpected<ResultType>(exception);
            }
        }
    }

}