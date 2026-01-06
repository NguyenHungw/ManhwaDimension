namespace ManhwaDimension.Util.Entities
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
        string Name { get; set; }

        bool Active { get; set; }
        DateTime CreatedAt { get; set; }
    }
    public interface IEntityBase
    {
        long Id { get; set; }
        string Name { get; set; }
        bool Active { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
