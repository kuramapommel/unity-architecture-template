using System;
using System.Collections.Generic;
using Domain.Exceptions;

namespace Domain
{
    /// <summary>
    /// ValueObject であることを表す interface
    /// </summary>
    /// <typeparam name="ValueType">値の実際の型</typeparam>
    public interface IValueObject<ValueType>
    {
        /// <summary>
        /// 値
        /// </summary>
        /// <value>The value.</value>
        ValueType Value { get; }
    }

    /// <summary>
    /// IValueObject 実装クラス向け、拡張メソッドクラス
    /// </summary>
    /// IValueObject<ValueType> 実装の拡張メソッドを同ファイル、同 namespace 内に定義しておくことで
    /// 該当のクラスは特別何かをインポートすることなく拡張機能を使うことができる
    public static class ValueObjectExt
    {
        /// <summary>
        /// 比較
        /// </summary>
        /// <returns>The compare.</returns>
        /// <param name="self">Self.</param>
        /// <param name="that">That.</param>
        /// <remarks>
        /// object 型との比較
        /// that が以下の条件に該当する場合は比較せずに ArgumentException を投げる
        /// * null のとき
        /// * IComparable<DateTime> 型ではない場合
        /// * IValueObject<DateTime> 型ではない場合
        /// </remarks>
        public static int Compare(this IValueObject<DateTime> self, object that)
        {
            if (that == null || !(that is IComparable<DateTime>) || !(that is IValueObject<DateTime>)) throw new ArgumentException();

            return self.Value.CompareTo(((IValueObject<DateTime>)that).Value);
        }
    }
}

namespace Domain.ValueObject.Requires
{
    /// <summary>
    /// バリデータ
    /// </summary>
    public interface IValidator<Value>
    {
        /// <summary>
        /// バリデーションチェック
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="value">Value.</param>
        bool Validate(Value value);
    }
}

namespace Domain.ValueObject.Requires.Int
{
    /// <summary>
    /// int validator
    /// </summary>
    public interface IIntValidator : IValidator<int>
    {
    }

    /// <summary>
    /// 長さ
    /// </summary>
    public readonly struct Length : IIntValidator
    {
        /// <summary>
        /// 最低値
        /// </summary>
        /// <value>The minimum.</value>
        private int Min { get; }

        /// <summary>
        /// 最大値
        /// </summary>
        /// <value>The max.</value>
        private int Max { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public Length(int min = int.MinValue, int max = int.MaxValue) => (Min, Max) = (min, max);

        /// <summary>
        /// バリデーション
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="value">Value.</param>
        public bool Validate(int value) => (Min <= value && value <= Max);
    }

        /// <summary>
        /// int 用バリデーションチェッククラス
        /// </summary>
    public static class IntExt
    {
        /// <summary>
        /// バリデーションチェックを適用する
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="validators">Validators.</param>
        /// <typeparam name="V">The 1st type parameter.</typeparam>
        public static void Require<V>(this int value, params V[] validators)
            where V : IIntValidator
        {
            foreach(var validator in validators)
            {
                if (!validator.Validate(value)) throw new ValidationException();
            }
        }

        /// <summary>
        /// バリデーションチェックを適用する
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="validators">Validators.</param>
        /// <typeparam name="V">The 1st type parameter.</typeparam>
        public static void Require<V>(this int value, IEnumerable<V> validators)
            where V : IIntValidator
        {
            foreach (var validator in validators)
            {
                if (!validator.Validate(value)) throw new ValidationException();
            }
        }
    }
}

namespace Domain.ValueObject.Requires.Long
{
    /// <summary>
    /// Long validator.
    /// </summary>
    public interface ILongValidator : IValidator<long>
    {
    }

    /// <summary>
    /// 長さ
    /// </summary>
    public readonly struct Length : ILongValidator
    {
        /// <summary>
        /// 最小値
        /// </summary>
        /// <value>The minimum.</value>
        private long Min { get; }

        /// <summary>
        /// 最大値
        /// </summary>
        /// <value>The max.</value>
        private long Max { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public Length(long min = long.MinValue, long max = long.MaxValue) => (Min, Max) = (min, max);

        /// <summary>
        /// バリデーション
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="value">Value.</param>
        public bool Validate(long value) => (Min <= value && value <= Max);
    }

    /// <summary>
    /// int 用バリデーションチェッククラス
    /// </summary>
    public static class LongExt
    {
        /// <summary>
        /// バリデーションチェックを適用する
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="validators">Validators.</param>
        /// <typeparam name="V">The 1st type parameter.</typeparam>
        public static void Require<V>(this long value, params V[] validators)
            where V : ILongValidator
        {
            foreach (var validator in validators)
            {
                if (!validator.Validate(value)) throw new ValidationException();
            }
        }

        /// <summary>
        /// バリデーションチェックを適用する
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="validators">Validators.</param>
        /// <typeparam name="V">The 1st type parameter.</typeparam>
        public static void Require<V>(this long value, IEnumerable<V> validators)
            where V : ILongValidator
        {
            foreach (var validator in validators)
            {
                if (!validator.Validate(value)) throw new ValidationException();
            }
        }
    }
}
