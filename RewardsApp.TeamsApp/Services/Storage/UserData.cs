using Azure;
using Azure.Data.Tables;
using System;

namespace RewardsApp.TeamsApp.Services.Storage
{
    public class UserData : ITableEntity
    {
        public string UserId
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string PartitionKey { get; set; } = default!;

        public string RowKey { get; set; } = string.Empty;

        public string WalletId { get; set; } = string.Empty;

        public ETag ETag { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}