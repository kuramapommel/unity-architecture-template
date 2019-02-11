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
            var domainResult = DomainResult.Failure(domainError);

            foreach (var error in ((Failure)domainResult).Errors)
            {
                AreEqual(domainError, error);
            }
        }

        [Test]
        public void DomainResultはコンストラクタで注入した成功情報を保持する()
        {
            var result = 0;
            var domainResult = DomainResult.Success(result);

            AreEqual(result, ((Success<int>)domainResult).Result);
        }
    }
}