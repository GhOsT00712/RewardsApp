using Azure.Data.Tables;

namespace RewardsApp.TeamsApp.Services.Storage
{
    public sealed class UserDataStoreFactory
    {
        public UserDataStoreFactory()
        {
        }

        private static UserDataStoreFactory instance = null;

        public static UserDataStoreFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserDataStoreFactory();
                }
                return instance;
            }
        }


        public UserDataStore GetUserDataStore() {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=hacktable-db;AccountKey=avNvpJakzTfzBgcBbps413YHX2ykpbRPf7S7ZcHBL99Yt4GqgzbIAouoxotyvRf7DocgDYRboBTk7AEek653qQ==;TableEndpoint=https://hacktable-db.table.cosmos.azure.com:443/;";
            TableServiceClient tableServiceClient = new TableServiceClient(connectionString);
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "adventureworks"
            );
            UserDataStore userDataStore = new UserDataStore(tableClient);
            return userDataStore;
        }


    }
}
