using System;
using Domain.Exceptions;

namespace Domain
{
    public static class DomainResultExt
    {
        public static IDomainResult<Result> FlatMap<Source, Result>(this IDomainResult<Source> source, Func<Source, IDomainResult<Result>> selector)
        {
            if (source == null) throw new UnexpectedException();

            if (selector == null) throw new UnexpectedException();

            switch (source)
            {
                case Failure<Source> failure:
                    return DomainResult.Failure<Result>(failure.Reason);

                case Success<Source> success:
                    return selector(success.Result);
            }

            throw new UnexpectedException();
        }

        public static IDomainResult<Result> Map<Source, Result>(this IDomainResult<Source> source, Func<Source, Result> selector)
        {
            if (source == null) throw new UnexpectedException();

            if (selector == null) throw new UnexpectedException();

            switch (source)
            {
                case Failure<Source> failure:
                    return DomainResult.Failure<Result>(failure.Reason);

                case Success<Source> success:
                    return DomainResult.Success(selector(success.Result));
            }

            throw new UnexpectedException();
        }
    }
}
