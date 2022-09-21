using System.Threading.Tasks;

namespace RewardsApp.TeamsApp.Services.Storage
{
    public interface IUserDataStore
    {
        /// <summary>
        /// Persist the user's token.
        /// </summary>
        /// <param name="tenantId">The user's tenant id.</param>
        /// <param name="userId"The user's id.></param>
        /// <param name="token"The user's token.></param>
        /// <returns>A Task that resolves once the operation is complete.</returns>
        void SetUserData(string userId, string walletId);

        /// <summary>
        /// Expunge the user's token from storage.
        /// </summary>
        /// <param name="tenantId">The user's tenant id.</param>
        /// <param name="userId"The user's id.></param>
        /// <returns>A Task that resolves once the operation is complete.</returns>
        void DeleteUserData(string userId);
        /// <summary>
        /// Retrieve the user's token from storage.
        /// </summary>
        /// <param name="tenantId">The user's tenant id.</param>
        /// <param name="userId"The user's id.></param>
        /// <returns>A Task that resolves to the user's opaque token string or null if the token isn't in storage.</returns>
        string GetUserWallet(string userId);
    }
}