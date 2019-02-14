using Domain;
using System;

namespace UseCase
{
    /// <summary>
    /// アプリケーション層で発生した例外を包むための構造体
    /// </summary>
    public readonly struct ApplicationError
    {
        public ApplicationError(IDomainError domainError) { }
    }

    public static class DomainErrorExt
    {
        public static ApplicationError ToApplicationError(this IDomainError domainError) => new ApplicationError(domainError);
    }
}