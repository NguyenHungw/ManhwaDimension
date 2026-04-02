namespace ManhwaDimension.Util.Entities
{
    /// <summary>
    /// Abstract base chỉ có Id + CreatedAt
    /// </summary>
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    /// <summary>
    /// Abstract base đầy đủ — cho entity có Id, Name, Active, CreatedAt
    /// </summary>
    public abstract class EntityFull<TKey> : EntityBase<TKey>, IHasActive, IHasName
    {
        public string Name { get; set; } = null!;
        public bool Active { get; set; }
    }
}
