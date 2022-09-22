using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Data.Tables;
using Bogus.DataSets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RewardsApp.TeamsApp.Services.Storage;
using RewardsApp.TeamsApp.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace RewardsApp.TeamsApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
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

        [Route("showNFT")]
        [HttpPost]
        public async Task<ActionResult> ShowNFT(string emailId)
        {
            try
            {
                emailId = "dubeypiyush@microsoft.com";
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=hacktable-db;AccountKey=avNvpJakzTfzBgcBbps413YHX2ykpbRPf7S7ZcHBL99Yt4GqgzbIAouoxotyvRf7DocgDYRboBTk7AEek653qQ==;TableEndpoint=https://hacktable-db.table.cosmos.azure.com:443/;";
                TableServiceClient tableServiceClient = new TableServiceClient(connectionString);
                TableClient tableClient = tableServiceClient.GetTableClient(
                    tableName: "adventureworks"
                );

                UserDataStore userDataStore = new UserDataStore(tableClient);
                var savedWalletId = userDataStore.GetUserWallet(emailId);


                ViewData["walletId"] = savedWalletId;
                ViewData["userId"] = emailId;
                HashSet<Dictionary<String, String>> nftMetaData = await this.GetNFTData(savedWalletId);
                ViewData["nftMetaData"] = nftMetaData;



                return View("Nft");
            }
            catch
            {
                return View();
            }

        }

        public async Task<HashSet<Dictionary<String, String>>> GetNFTData(String walletId)
        {
            NFTData nftData = new NFTData();
            string baseurl = "http://nftdapp.azurewebsites.net/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("get/0xf32857eA7c345B2b245c6e9864Af1e7716Df7b1e");
                if (Res.IsSuccessStatusCode)
                {
                    var userResponse = Res.Content.ReadAsStringAsync().Result;
                    nftData = JsonConvert.DeserializeObject<NFTData>(userResponse);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)Res.StatusCode,
                                  Res.ReasonPhrase);
                }

                List<OwnedNft> ownedNfts = nftData.ownedNfts;
                HashSet<Dictionary<String, String>> result = new HashSet<Dictionary<string, string>>();
                foreach (OwnedNft ownedNft in ownedNfts)
                {
                    Dictionary<String, String> metaData = new Dictionary<String, String>();
                    metaData.Add("image", ownedNft.rawMetadata?.image);
                    foreach(var attibute in ownedNft.rawMetadata.attributes)
                    metaData.Add(attibute.trait_type, attibute.value);
                    result.Add(metaData);
                }
                return result;
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