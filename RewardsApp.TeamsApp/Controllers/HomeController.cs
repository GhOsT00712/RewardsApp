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
            ViewBag.AboutApp = "Reward your Teammated with NFTs.";
            return View();
        }

        [Route("")]
        [HttpPost]
        public ActionResult Register(string walletId, string emailId)
        {
            try
            {
                UserDataStore userDataStore = UserDataStoreFactory.Instance.GetUserDataStore();
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
        public async Task<ActionResult> ShowNFT(string emailIdNft)
        {
            try
            {
                UserDataStore userDataStore = UserDataStoreFactory.Instance.GetUserDataStore();
                var savedWalletId = userDataStore.GetUserWallet(emailIdNft);

                ViewData["walletId"] = savedWalletId;
                ViewData["userId"] = emailIdNft;
                HashSet<Dictionary<String, String>> nftMetaData = await this.GetNFTData(savedWalletId);
                ViewData["nftMetaData"] = nftMetaData;
                return View("Index");
            }
            catch
            {
                return View("Index");
            }

        }

        [Route("transfer")]
        [HttpPost]
        public async Task<ActionResult> Transfer(string emailIdTrans, string selectedEmailId, string rewardType)
        {
            try
            {
                emailIdTrans = emailIdTrans?.ToLower();
                selectedEmailId = selectedEmailId?.ToLower();
                string transferError = null;
                string transferSuccess = null;

                string mintToken;

                UserDataStore userDataStore = UserDataStoreFactory.Instance.GetUserDataStore();
                var userWallet = userDataStore.GetUserWallet(emailIdTrans);
                var selecteduserWallet = userDataStore.GetUserWallet(selectedEmailId);

                if (userWallet == null)
                {
                    transferError = "You don't have linked wallet.";
                }
                else if (selecteduserWallet == null)
                {
                    transferError = "Selected user " + selectedEmailId + " doesn't have linked wallet.";
                }
                else {

                    mintToken = await this.GetMintToken(rewardType);
                    string response = await this.TrafnsferMintToken(mintToken, selecteduserWallet);
                    if (response == null) {
                        transferError = "Unable to transfer NFT. Please check your wallet Id.";
                    }
                    else {
                        transferSuccess = "Transfered NFT to selected user.";
                    }
                }

                ViewData["transferError"] = transferError;
                ViewData["transferSuccess"] = transferSuccess;

                return View("Index");
            }
            catch
            {
                return View("Index");
            }

        }

        public async Task<String> GetMintToken(string rewardType)
        {
            string mintToken = null;
            string baseurl = "https://nftdapp.azurewebsites.net/mint/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(rewardType, null);
                if (result.IsSuccessStatusCode)
                {
                    mintToken = result.Content.ReadAsStringAsync().Result;
                }
                else
                {
                }
            }
            return mintToken;
        }

        public async Task<String> TrafnsferMintToken(string mintToken, string walletId)
        {
            walletId = "0x6c62693e39629A2D3B4f7Fc4e34C8758ae261B3C"; //remove hard coded data
            string response = null;
            string baseurl = "https://nftdapp.azurewebsites.net/transfer/";
            string uri = mintToken + "/" + walletId;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync(uri, null);
                if (result.IsSuccessStatusCode)
                {
                    response = result.Content.ReadAsStringAsync().Result;
                }
            }
            return response;
        }


        public async Task<HashSet<Dictionary<String, String>>> GetNFTData(String walletId)
        {
            walletId = "0x6c62693e39629A2D3B4f7Fc4e34C8758ae261B3C"; //remove hard coded data
            string uri = "get/"+walletId;
            NFTData nftData = new NFTData();
            string baseurl = "http://nftdapp.azurewebsites.net/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(uri);
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
                    metaData.Add("name", ownedNft.rawMetadata?.name);
                    metaData.Add("description", ownedNft.rawMetadata?.description);
                    metaData.Add("image", ownedNft.rawMetadata?.image);
                    result.Add(metaData);
                }
                return result;
            }
        }


        [Route("MyWallet")]
        public string MyWallet()
        {
            UserDataStore userDataStore = UserDataStoreFactory.Instance.GetUserDataStore();
            string userId = "vishnugupta@microsoft.com"; // remove hard coded data
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