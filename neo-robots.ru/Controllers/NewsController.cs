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
    public class NewsController : Controller
    {
        private readonly INewsManager _newsMgr;
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger, INewsManager newsMgr)
        {
            _logger = logger;
            _newsMgr = newsMgr;
        }

        public IActionResult Index(NewsPage m)
        {
            m = _newsMgr.GetNewsPage(m);
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
