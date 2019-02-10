using NUnit.Framework;
using Domain;
using System;
using static NUnit.Framework.Assert;

namespace Test.Domain
{
    public sealed class ValueObjectSpec
    {
        [Test]
        public void IValueObject_DateTimeの拡張メソッドCompareは日付比較できる()
        {
            var datetimeMin = DateTime.MinValue;
            var datetimeMax = DateTime.MaxValue;

            var min1 = new DateTimeValueObjectMock(datetimeMin);
            var min2 = new DateTimeValueObjectMock(datetimeMin);

            var max = new DateTimeValueObjectMock(datetimeMax);

            var resultMin1CompareToMin2 = min1.Compare(min2);
            var resultMin2CompareToMin1 = min2.Compare(min1);

            var resultMin1CompareToMax = min1.Compare(max);
            var resultMaxCompareToMin1 = max.Compare(min1);

            AreEqual(0, resultMin1CompareToMin2);
            AreEqual(0, resultMin2CompareToMin1);
            AreEqual(-1, resultMin1CompareToMax); // 古い日時は新しい日時 `より前` のため -1
            AreEqual(1, resultMaxCompareToMin1); // 新しい日時は古い日時 `より後` のため 1
        }

        #region mock 定義
        /// <summary>
        /// テスト用の mock
        /// </summary>
        private readonly struct DateTimeValueObjectMock : IValueObject<DateTime>, IComparable<DateTime>
        {
            public DateTime Value { get; }

            public DateTimeValueObjectMock(DateTime datetime) => Value = datetime;

            public int CompareTo(DateTime that) => Value.CompareTo(that);
        }
        #endregion
    }
}
