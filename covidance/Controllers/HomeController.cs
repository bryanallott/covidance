using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using covidance.Models;
using covidance.Lib.Communications;

namespace covidance.Controllers
{
#if !DEBUG
    [RequireHttps]
#endif
    public class HomeController : Controller
    {
        private readonly IMyEmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IMyEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
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
