using Handin3Database.Models;
using Handin3Database.Repository;
using Handin3Database.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Handin3Database.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TweetRepository _tweetRepository;

        public HomeController(ILogger<HomeController> logger, TweetRepository tweetRepository)
        {
            _logger = logger;
            _tweetRepository = tweetRepository;
        }

        public async Task<IActionResult> Index()
        {
            _tweetRepository.Test();
            var test = await _tweetRepository.GetTweets();
            var list = await _tweetRepository.GetMapReducedTweets();

            TweetViewModel tvm = new TweetViewModel()
            {
                Tweets = list
            };

            return View(tvm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
