using System;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Recurrence
    {        
        public required string AccountName { get; set; }
        public required virtual Account Account { get; set; }

        public required string Name { get; set; }

        public double Value { get; set; }
        public bool IsRate { get; set; }
        public string? Description { get; set; }

        public TimeSpan TimeSpan { get; set; }
        public DateTime LastApplied { get; set; }
    }
}