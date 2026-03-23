using System;
using System.Collections.Generic;

namespace ManhwaDimension.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public long UserId { get; set; }
        public int BalanceCents { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
