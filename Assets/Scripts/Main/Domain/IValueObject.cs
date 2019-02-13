using System;
using System.Linq;

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

namespace Domain.ValueObject.Attributes
{
    /// <summary>
    /// ValueObject チェック用 custom attribute interface
    /// </summary>
    public interface IRequireAttribute<ValueType>
    {
        /// <summary>
        /// バリデーションチェック
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="value">Value.</param>
        bool Validate(ValueType value);
    }

    /// <summary>
    /// int 値の length を制限するためのバリデーション用 custom attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequireIntLengthAttribute : Attribute, IRequireAttribute<int>
    {
        /// <summary>
        /// 最低値
        /// </summary>
        /// <value>The minimum.</value>
        public int Min { get; }

        /// <summary>
        /// 最大値
        /// </summary>
        /// <value>The max.</value>
        public int Max { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public RequireIntLengthAttribute(int min = int.MinValue, int max = int.MaxValue) => (Min, Max) = (min, max);

        /// <summary>
        /// バリデーションチェック
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="value">Value.</param>
        public bool Validate(int value) => Min <= value && value <= Max;
    }

    /// <summary>
    /// long 値の length を制限するためのバリデーション用 custom attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequireLongLengthAttribute : Attribute, IRequireAttribute<long>
    {
        /// <summary>
        /// 最低値
        /// </summary>
        /// <value>The minimum.</value>
        public long Min { get; }

        /// <summary>
        /// 最大値
        /// </summary>
        /// <value>The max.</value>
        public long Max { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public RequireLongLengthAttribute(long min = long.MinValue, long max = long.MaxValue) => (Min, Max) = (min, max);

        /// <summary>
        /// バリデーションチェック
        /// </summary>
        /// <returns>The validate.</returns>
        /// <param name="value">Value.</param>
        public bool Validate(long value) => Min <= value && value <= Max;
    }

    /// <summary>
    /// value object の値の拡張メソッドクラス
    /// </summary>
    public static class ValueExt
    {
        /// <summary>
        /// custom attribute を利用したバリデーションチェック
        /// </summary>
        /// <returns>The validated.</returns>
        /// <param name="value">Value.</param>
        /// <typeparam name="ValueObjectType">The 1st type parameter.</typeparam>
        /// <typeparam name="AttributeType">The 2nd type parameter.</typeparam>
        /// <typeparam name="ValueType">The 3rd type parameter.</typeparam>
        /// <remarks>バリデーションエラー時は ArgumentException を投げる</remarks>
        public static ValueType Validated<ValueObjectType, AttributeType, ValueType>(this ValueType value)
            where ValueObjectType : IValueObject<ValueType>
            where AttributeType : class, IRequireAttribute<ValueType> =>
            typeof(ValueObjectType)
                .GetProperties()
                .SelectMany(property => property.GetCustomAttributes(typeof(AttributeType), false))
                .FirstOrDefault() is AttributeType attribute && attribute.Validate(value) ? value : throw new ArgumentException();
    }
}