
using NuGet.Packaging.Signing;

namespace ManhwaDimension.Models.DTO.Comic
{
    // ComicDTO.cs
    public class ChapterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChapterNumber { get; set; }
        public int VolumeNumber { get; set; }
        public string Summary { get; set; }

    }
}
