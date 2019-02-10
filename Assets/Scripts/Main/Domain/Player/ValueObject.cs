/* 
 * ValueObject.cs にはその境界づけられたコンテキストで使用する ValueObject を定義する
 * ValueObject は immutable を担保するため readonly struct で定義する
 * 各 ValueObject は IValueObject を実装する
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Player
{
    /// <summary>
    /// プレイヤーを一意に識別するための識別子
    /// </summary>
    /// 識別子を表す ValueObject は IEntityId を実装する
    /// また IEntityId 実装の ValueObject は operator == を使用するため
    /// bool Equals(object obj) と int GetHashCode() を override する
    public readonly struct PlayerId : IEntityId, IValueObject<long>
    {
        /// <summary>
        /// 識別子の値
        /// </summary>
        /// <value>The value.</value>
        public long Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">Value.</param>
        public PlayerId(long value) => Value = value;

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>同じIDならtrue</returns>
        public override bool Equals(object obj) => obj is PlayerId && Value == ((PlayerId)obj).Value;

        /// <summary>
        /// hash取得
        /// </summary>
        /// <returns>hash</returns>
        public override int GetHashCode() => EqualityComparer<long>.Default.GetHashCode(Value);

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerId"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PlayerId self, object that) => self.Equals(that);

        /// <summary>
        /// 非等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerId"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PlayerId self, object that) => !(self == that);
    }

    /// <summary>
    /// プレイヤー名
    /// </summary>
    public readonly struct PlayerName : IValueObject<string>
    {
        /// <summary>
        /// プレイヤー名の値
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">Value.</param>
        public PlayerName(string value) => Value = value;
    }

    /// <summary>
    /// プレイヤー作成日時
    /// </summary>
    public readonly struct PlayerCreateAt : IValueObject<DateTime>, IComparable, IComparable<DateTime>
    {
        /// <summary>
        /// プレイヤー作成日時の値
        /// </summary>
        /// <value>The value.</value>
        public DateTime Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">Value.</param>
        public PlayerCreateAt(DateTime value) => Value = value;

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>同じIDならtrue</returns>
        public override bool Equals(object obj) => obj is IValueObject<DateTime> && Value.Equals(((IValueObject<DateTime>)obj).Value);

        /// <summary>
        /// hash取得
        /// </summary>
        /// <returns>hash</returns>
        public override int GetHashCode() => EqualityComparer<DateTime>.Default.GetHashCode(Value);

        /// <summary>
        /// 比較
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="that">That.</param>
        public int CompareTo(DateTime that) => Value.CompareTo(that);

        /// <summary>
        /// 比較
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="that">That.</param>
        public int CompareTo(object that) => this.Compare(that);

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerCreateAt"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PlayerCreateAt self, object that) => self.Equals(that);

        /// <summary>
        /// 非等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerCreateAt"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PlayerCreateAt self, object that) => !(self == that);
    }

    /// <summary>
    /// プレイヤー更新日時
    /// </summary>
    public readonly struct PlayerUpdateAt : IValueObject<DateTime>, IComparable, IComparable<DateTime>
    {
        /// <summary>
        /// プレイヤー更新日時の値
        /// </summary>
        /// <value>The value.</value>
        public DateTime Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">Value.</param>
        public PlayerUpdateAt(DateTime value) => Value = value;

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>同じIDならtrue</returns>
        public override bool Equals(object obj) => obj is IValueObject<DateTime> && Value.Equals(((IValueObject<DateTime>)obj).Value);

        /// <summary>
        /// hash取得
        /// </summary>
        /// <returns>hash</returns>
        public override int GetHashCode() => EqualityComparer<DateTime>.Default.GetHashCode(Value);

        /// <summary>
        /// 比較
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="that">That.</param>
        public int CompareTo(DateTime that) => Value.CompareTo(that);

        /// <summary>
        /// 比較
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="that">That.</param>
        public int CompareTo(object that) => this.Compare(that);

        /// <summary>
        /// PlayerCreateAtをPlayerUpdateAtに変換する暗黙の型変換
        /// </summary>
        /// <returns>The implicit.</returns>
        /// <param name="createAt">Create at.</param>
        public static implicit operator PlayerUpdateAt(PlayerCreateAt createAt) => new PlayerUpdateAt(createAt.Value);

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerUpdateAt"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PlayerUpdateAt self, object that) => self.Equals(that);

        /// <summary>
        /// 非等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerUpdateAt"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PlayerUpdateAt self, object that) => !(self == that);
    }
}