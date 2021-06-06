using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Account
    {
        [Required, Key]
        public string Name { get; set; }

        [Required]
        public double Balance { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new();
        public virtual List<Recurrence> Recurrences { get; set; } = new();
    }
}