using System;

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