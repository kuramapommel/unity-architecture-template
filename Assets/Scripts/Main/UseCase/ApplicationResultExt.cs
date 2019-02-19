using System;

namespace UseCase
{
    /// <summary>
    /// Application result ext.
    /// </summary>
    public static class ApplicationResultExt
    {
        /// <summary>
        /// Flats the map.
        /// </summary>
        /// <returns>The map.</returns>
        /// <param name="source">Source.</param>
        /// <param name="selector">Selector.</param>
        /// <typeparam name="Source">The 1st type parameter.</typeparam>
        /// <typeparam name="Result">The 2nd type parameter.</typeparam>
        public static IApplicationResult<Result> FlatMap<Source, Result>(this IApplicationResult<Source> source, Func<Source, IApplicationResult<Result>> selector)
        {
            if (source == null) return ApplicationResult.Unexpected<Result>();

            if (selector == null) return ApplicationResult.Unexpected<Result>();

            switch (source)
            {
                case Failure<Source> failure:
                    return ApplicationResult.Failure<Result>(failure.Reason);

                case Success<Source> success:
                    return selector(success.Result);
            }

            return ApplicationResult.Unexpected<Result>();
        }

        /// <summary>
        /// Map the specified source and selector.
        /// </summary>
        /// <returns>The map.</returns>
        /// <param name="source">Source.</param>
        /// <param name="selector">Selector.</param>
        /// <typeparam name="Source">The 1st type parameter.</typeparam>
        /// <typeparam name="Result">The 2nd type parameter.</typeparam>
        public static IApplicationResult<Result> Map<Source, Result>(this IApplicationResult<Source> source, Func<Source, Result> selector)
        {
            if (source == null) return ApplicationResult.Unexpected<Result>();

            if (selector == null) return ApplicationResult.Unexpected<Result>();

            switch (source)
            {
                case Failure<Source> failure:
                    return ApplicationResult.Failure<Result>(failure.Reason);

                case Success<Source> success:
                    return ApplicationResult.Success(selector(success.Result));
            }

            return ApplicationResult.Unexpected<Result>();
        }
    }
}
