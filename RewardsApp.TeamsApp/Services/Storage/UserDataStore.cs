using Azure.Data.Tables;
using Azure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace RewardsApp.TeamsApp.Services.Storage
{
    public sealed class UserDataStore : IUserDataStore
    {
        private readonly TableClient _tableClient;
        private readonly string partitionKey = "gear-surf-surfboards";

        public UserDataStore(
            TableClient tableClient)
        {
            _tableClient = tableClient;
        }

        public void SetUserData(string userId, string walletId)
        {
            if (userId == null)
            {
                return;
            }

            _tableClient.CreateIfNotExists();
            var entity = new UserData
            {
                UserId = userId,
                WalletId = walletId,
                PartitionKey = this.partitionKey

            };
            _tableClient.UpsertEntity(entity);
        }

        public void DeleteUserData(string userId)
        {
            if (userId == null)
            {
                return;
            }

            _tableClient.CreateIfNotExists();
            _tableClient.DeleteEntity(partitionKey: this.partitionKey, rowKey: userId);
        }

        public string GetUserWallet(string userId)
        {
            if (userId == null) { 
                return null;
            }

            _tableClient.CreateIfNotExists();
            try
            {
                var response = _tableClient.GetEntity<UserData>(partitionKey: this.partitionKey, rowKey: userId);
                return response?.Value?.WalletId;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }
    }
}