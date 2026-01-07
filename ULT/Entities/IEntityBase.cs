namespace ManhwaDimension.Util.Entities
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
        string Name { get; set; }

        bool Active { get; set; }
        DateTimeOffset CreatedAt { get; set; }
    }
    //public interface IEntityBase
    //{
    //    int Id { get; set; }
    //    string Name { get; set; }
    //    bool Active { get; set; }
    //    DateTimeOffset CreatedAt { get; set; }
    //}
}
