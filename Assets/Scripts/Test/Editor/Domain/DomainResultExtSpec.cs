using NUnit.Framework;
using Domain;
using System;
using static NUnit.Framework.Assert;

namespace Test.Domain
{
    public sealed class DomainResultExtSpec
    {
        [Test]
        public void すべての処理がSuccessの場合最終結果もSuccessになる()
        {
            var num1 = 1;
            var num2 = 2;
            var num3 = 3;

            // FlatMapのテスト
            foreach (var result in DomainResult.Success(num1)
                .FlatMap(value => DomainResult.Success(num2 + value))
                .FlatMap(value => DomainResult.Success(num3 + value)))
            {
                AreEqual(num1 + num2 + num3, result);
            }

            // Mapのテスト
            foreach (var result in DomainResult.Success(num1 + num2)
                .Map(value => num3 + value))
            {
                AreEqual(num1 + num2 + num3, result);
            }
        }


        [Test]
        public void ひとつでもFailureの場合最終結果もFailureになる()
        {
            var num1 = 1;
            var num3 = 3;

            var error = new DomainErrrorMock("error");

            // FlatMapのテスト
            if (DomainResult.Success(num1)
                .FlatMap(value => DomainResult.Failure<int>(error))
                .FlatMap(value => DomainResult.Success(num3 + value)) is Failure<int> flatmapFailure)
            {
                AreEqual(error, flatmapFailure.Reason);
            }
            else
            {
                Fail();
            }

            // Mapのテスト
            if (DomainResult.Failure<int>(error)
                .Map(value => num3 + value) is Failure<int> mapFailure)
            {
                AreEqual(error, mapFailure.Reason);
            }
            else
            {
                Fail();
            }
        }

        private readonly struct DomainErrrorMock : IDomainError
        {
            public string Message { get; }

            public ErrorLevel Level => ErrorLevel.ERROR;

            public Exception Exception => null;

            public DomainErrrorMock(string message) => Message = message;
        }
    }
}