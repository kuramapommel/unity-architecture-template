namespace Domain
{
    /// <summary>
    /// entity であることを表す interface
    /// </summary>
    /// すべての entity はこの interface を実装する
    /// <typeparam name="EntityId">識別子の型</typeparam>
    public interface IEntity<EntityId> where EntityId : IEntityId
    {
        /// <summary>
        /// 一意性を表す識別子
        /// </summary>
        /// <value>The identifier.</value>
        EntityId Id { get; }
    }

    /// <summary>
    /// entity の一意性を表す識別子であることを表す interface
    /// </summary>
    public interface IEntityId
    {
    }
}