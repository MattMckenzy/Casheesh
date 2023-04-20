using System;

namespace Casheesh.Models
{
    public class Balance
    {
        public required string AccountName { get; set; }
        public required virtual Account Account { get; set; }

        public int Id { get; set; }

        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}