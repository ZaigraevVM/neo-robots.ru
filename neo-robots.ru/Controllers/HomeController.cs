using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMI.Managers;
using SMI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsManager _newsMgr;
        private readonly ILogger<HomeController> _logger;
        private readonly IAggregatorManager _aggregator;

        public HomeController(ILogger<HomeController> logger, INewsManager newsMgr, IAggregatorManager aggregator)
        {
            _logger = logger;
            _newsMgr = newsMgr;
            _aggregator = aggregator;
        }

        public async Task<IActionResult> Index(HomePage m)
        {
            await _aggregator.ImportNewsAsync();
            m = _newsMgr.GetHomePage(m);
            return View(m);
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
