
using System;
using NUnit.Framework;
using UseCase;
using static NUnit.Framework.Assert;
using BaseUseCase = UseCase.UseCase<int, int>;
using DomainResult = Domain.DomainResult;
using ErrorLevel = Domain.ErrorLevel;
using Failure = UseCase.Failure<int>;
using IDomainResult = Domain.IDomainResult<int>;
using ValidationError = Domain.ValidationError;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Test.UseCase
{
    public sealed class UseCaseSpec
    {
        private const string ValidationErrorMessage = "validation error!!";

        [Test]
        public void バリデーションエラーの場合Failureを返す()
        {
            var usecase = new ErrorUseCaseMock();
            var protocol = new ValidationErrorProtocol();

            var result = usecase.Execute(protocol);

            if (result is Failure failure)
            {
                AreEqual(ValidationErrorMessage, failure.Reason.Message);
            }
            else
            {
                Fail();
            }
        }

        [Test]
        public void ユースケース実行中に想定外の例外が発生した場合Failureを返す()
        {
            var usecase = new ErrorUseCaseMock();
            var protocol = new ValidationOKProtocol();

            var result = usecase.Execute(protocol);

            if (result is Failure failure)
            {
                AreEqual("想定外の例外が発生しました", failure.Reason.Message);
            }
            else
            {
                Fail();
            }
        }

        private sealed class ErrorUseCaseMock : BaseUseCase
        {
            protected override IApplicationResult<int> ExecuteImpl(int protocol) =>
                throw new NotImplementedException();
        }

        private readonly struct ValidationErrorProtocol : IProtocol<int>
        {
            public IDomainResult ToUseCaseProtocol() => DomainResult.Failure<int>(new ValidationError(new ValidationException(ValidationErrorMessage), ErrorLevel.WARNING));
        }

        private readonly struct ValidationOKProtocol : IProtocol<int>
        {
            public IDomainResult ToUseCaseProtocol() => DomainResult.Success(1);
        }
    }
}
