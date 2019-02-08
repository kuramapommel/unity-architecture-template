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
}