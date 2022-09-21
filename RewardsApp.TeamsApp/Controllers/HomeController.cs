using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Data.Tables;
using Bogus.DataSets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RewardsApp.TeamsApp.Services.Storage;


namespace RewardsApp.TeamsApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.AboutApp = "Reward your Teammated with NFTs.";
            return View();
        }

        [Route("")]
        [HttpPost]
        public ActionResult Register(string walletId, string emailId)
        {
            try
            {
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=hacktable-db;AccountKey=avNvpJakzTfzBgcBbps413YHX2ykpbRPf7S7ZcHBL99Yt4GqgzbIAouoxotyvRf7DocgDYRboBTk7AEek653qQ==;TableEndpoint=https://hacktable-db.table.cosmos.azure.com:443/;";
                TableServiceClient tableServiceClient = new TableServiceClient(connectionString);
                TableClient tableClient = tableServiceClient.GetTableClient(
                    tableName: "adventureworks"
                );

                UserDataStore userDataStore = new UserDataStore(tableClient);
                if (!String.IsNullOrEmpty(walletId) && !String.IsNullOrEmpty(emailId))
                {
                    userDataStore.SetUserData(emailId, walletId);
                }
                var savedWalletId = userDataStore.GetUserWallet(emailId);


                ViewData["walletId"] = savedWalletId;
                ViewData["userId"] = emailId;
                return View("Index");
            }
            catch
            {
                return View();
            }

        }

        [Route("MyWallet")]
        public string MyWallet()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=hacktable-db;AccountKey=avNvpJakzTfzBgcBbps413YHX2ykpbRPf7S7ZcHBL99Yt4GqgzbIAouoxotyvRf7DocgDYRboBTk7AEek653qQ==;TableEndpoint=https://hacktable-db.table.cosmos.azure.com:443/;";
            TableServiceClient tableServiceClient = new TableServiceClient(connectionString);
            TableClient tableClient = tableServiceClient.GetTableClient(
                tableName: "adventureworks"
            );
            UserDataStore userDataStore = new UserDataStore(tableClient);
            string userId = "vishnugupta@microsoft.com";
            var savedWalletId = userDataStore.GetUserWallet(userId);
            return savedWalletId;
        }

        [Route("first")]
        public ActionResult First()
        {
            return View();
        }

        [Route("second")]
        public ActionResult Second()
        {
            return View();
        }

        [Route("configure")]
        public ActionResult Configure()
        {
            return View();
        }

    }
}