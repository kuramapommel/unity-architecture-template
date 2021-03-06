using System;
using Domain.Exceptions;

namespace Domain
{
    /// <summary>
    /// Domain result ext.
    /// </summary>
    public static class DomainResultExt
    {
        /// <summary>
        /// Flats the map.
        /// </summary>
        /// <returns>The map.</returns>
        /// <param name="source">Source.</param>
        /// <param name="selector">Selector.</param>
        /// <typeparam name="Source">The 1st type parameter.</typeparam>
        /// <typeparam name="Result">The 2nd type parameter.</typeparam>
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

        /// <summary>
        /// Map the specified source and selector.
        /// </summary>
        /// <returns>The map.</returns>
        /// <param name="source">Source.</param>
        /// <param name="selector">Selector.</param>
        /// <typeparam name="Source">The 1st type parameter.</typeparam>
        /// <typeparam name="Result">The 2nd type parameter.</typeparam>
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
