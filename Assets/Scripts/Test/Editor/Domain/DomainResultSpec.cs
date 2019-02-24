using System;
using Domain;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Test.Domain
{
    public sealed class DomainResultSpec
    {
        [Test]
        public void DomainResultはコンストラクタで注入したエラー情報を保持する()
        {
            var domainError = new DomainErrorMock();
            var domainResult = DomainResult.Failure<int>(domainError);


            if (domainResult is Failure<int> failure)
            {
                AreEqual(domainError, failure.Reason);
            }
            else
            {
                Fail();
            }
        }

        [Test]
        public void DomainResultはコンストラクタで注入した成功情報を保持する()
        {
            var expect = 0;
            var success = DomainResult.Success(expect);

            foreach (var result in success)
            {
                AreEqual(expect, result);
            }
        }

        private readonly struct DomainErrorMock : IDomainError
        {
            public string Message => "";

            public ErrorLevel Level => ErrorLevel.WARNING;

            public Exception Exception => new Exception();
        }
    }
}