using System;
using System.ComponentModel.DataAnnotations;

namespace Casheesh.Models
{
    public class Transaction
    {
        public required string AccountName { get; set; }
        public required virtual Account Account { get; set; }

        public required int Number { get; set; }

        public double Value { get; set; }
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}