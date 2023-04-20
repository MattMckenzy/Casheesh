using System;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Bounty
    {
        [Key]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public double Value { get; set; }
        public double BaseValue { get; set; }
        public double MaxValue { get; set; }

        public double IncrementValue { get; set; }
        public bool IsRate { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DateTime LastApplied { get; set; }
    }
}