using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string Type { get; set; } = null!;
        public int AmountCents { get; set; }
        public int BalanceAfter { get; set; }
        public string? Reference { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public virtual Wallet Wallet { get; set; } = null!;
    }
}
