using Azure;
using Azure.Data.Tables;
using System;

namespace RewardsApp.TeamsApp.Services.Storage
{
    public class Product : ITableEntity
    {
        public string UserId
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string PartitionKey { get; set; } = string.Empty;

        public string RowKey { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public bool Sale { get; set; }

        public ETag ETag { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}
