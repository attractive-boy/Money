// Models/ModificationRecord.cs
using SQLite;
using System;

    public class ModificationRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Order { get; set; }
        public decimal Amount { get; set; }
        public decimal Settlement { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }

    public class DailyConsumption
    {
        public string Username { get; set; }
        public decimal Consumption { get; set; }
    }

    public class OrderSummary
    {
        public string OrderGroup { get; set; }
        public decimal TotalAmount { get; set; }
    }

