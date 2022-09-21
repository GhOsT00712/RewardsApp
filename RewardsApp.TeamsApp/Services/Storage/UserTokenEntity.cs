using Azure;
using Azure.Data.Tables;
using System;

namespace RewardsApp.TeamsApp.Services.Storage
{
    public class UserTokenEntity : ITableEntity
    {
        public string TenantId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public string UserId
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string Token { get; set; } = string.Empty;

        public string PartitionKey { get; set; } = string.Empty;

        public string RowKey { get; set; } = string.Empty;

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
