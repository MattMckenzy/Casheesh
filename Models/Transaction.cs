using System;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Transaction
    {
        [Required]
        public string AccountName { get; set; }
        public virtual Account Account { get; set; }

        [Required]
        public int Number { get; set; }

        public double Value { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}