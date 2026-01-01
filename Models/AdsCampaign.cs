using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class AdsCampaign
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Advertiser { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int? BudgetCents { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
