namespace ManhwaDimension.Util.Entities
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
