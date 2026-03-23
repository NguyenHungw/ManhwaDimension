namespace ManhwaDimension.ULT.Entities
{
    public interface ISearchableEntity
    {
        public string Name { get; set; }
        public bool Active { get; set; }

        public string SearchText => Name;
    }
}
