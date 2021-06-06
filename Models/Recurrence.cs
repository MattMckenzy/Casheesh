using System;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Recurrence
    {
        [Required]
        public string AccountName { get; set; }
        public virtual Account Account { get; set; }

        [Required]
        public string Name { get; set; }

        public double Value { get; set; }
        public string Description { get; set; }

        public TimeSpan TimeSpan { get; set; }
        public DateTime LastApplied { get; set; }
    }
}