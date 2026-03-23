using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class SearchIndex
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = null!;
        public int EntityId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? TagsJson { get; set; }
        public string? Language { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
