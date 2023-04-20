using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Account
    {
        [Key]
        public required string Name { get; set; }

        public required double CurrentBalance { get; set; }

        public int Order { get; set; }

        public virtual List<Balance> Balances { get; set; } = new();
        public virtual List<Transaction> Transactions { get; set; } = new();
        public virtual List<Recurrence> Recurrences { get; set; } = new();
    }
}