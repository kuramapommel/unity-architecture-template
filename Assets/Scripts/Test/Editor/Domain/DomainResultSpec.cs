using NUnit.Framework;
using Domain;
using static NUnit.Framework.Assert;

namespace Test.Domain
{
    public sealed class DomainResultSpec
    {
        [Test]
        public void DomainResultはコンストラクタで注入したエラー情報を保持する()
        {
            var domainError = new DomainError();
            var domainResult = DomainResult.Failure<int>(domainError);

            foreach (var error in ((Failure<int>)domainResult).Errors)
            {
                AreEqual(domainError, error);
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
    }
}