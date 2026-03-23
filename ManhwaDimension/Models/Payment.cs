using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int AmountCents { get; set; }
        public string Currency { get; set; } = null!;
        public string? Provider { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
