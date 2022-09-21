using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace RestClient.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}



        //Hosted web API REST Service base url
        string Baseurl = "http://nftdapp.azurewebsites.net/";
        //string Baseurl = "https://ghost00712-nftreward-7595w46qfwwp7-5000.githubpreview.dev/";
        //string token = "0xed337f23dCb11b7b1e2939d14671A9E6487C8AdD";
        public async Task<ActionResult> Index()
        {
            // List<userData> UserInfo = new List<userData>();
            userData UserInfo = new userData();
            //List<OwnedNft> UserInfo = new List<OwnedNft>() ;

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("get/0xf32857eA7c345B2b245c6e9864Af1e7716Df7b1e");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    // Console.WriteLine("res content {0}", Res.Content);
                    //Storing the response details recieved from web api
                    var userResponse = Res.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("user response {0}", userResponse);
                  
                    //Deserializing the response recieved from web api and storing into the Employee list
                   UserInfo = JsonConvert.DeserializeObject<userData>(userResponse);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)Res.StatusCode,
                                  Res.ReasonPhrase);
                }
                //returning the employee list to view
                return View(UserInfo);
            }
        }
    }
}
