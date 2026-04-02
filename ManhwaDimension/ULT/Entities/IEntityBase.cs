namespace ManhwaDimension.Util.Entities
{
    /// <summary>
    /// Interface tối thiểu — chỉ cần Id + CreatedAt
    /// Mọi entity đều implement được
    /// </summary>
    public interface IEntityBase
    {
        int Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
    }

    /// <summary>
    /// Generic version cho entity có Id kiểu khác int (long, Guid...)
    /// </summary>
    public interface IEntityBase<T>
    {
        T Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
    }

    /// <summary>
    /// Entity có trường Active (soft delete)
    /// </summary>
    public interface IHasActive
    {
        bool Active { get; set; }
    }

    /// <summary>
    /// Entity có trường Name (searchable)
    /// </summary>
    public interface IHasName
    {
        string Name { get; set; }
    }

    /// <summary>
    /// Tương đương IEntityBase cũ — cho các entity có đủ Id, Name, Active, CreatedAt
    /// Dùng cho Author, Genre, Tag, Role...
    /// </summary>
    public interface IEntityFull : IEntityBase, IHasActive, IHasName
    {
    }
}
