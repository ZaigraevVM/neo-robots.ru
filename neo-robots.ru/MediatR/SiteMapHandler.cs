using MediatR;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using SMI.Data.Entities;
using System.Linq;
using System.Security.Policy;
using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SMI.MediatR
{
    public class SiteMapHandler : IRequestHandler<SiteMapRequest>
    {
        private readonly SmiContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SiteMapHandler(SmiContext context, IWebHostEnvironment webHostEnvironment) {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Handle(SiteMapRequest request, CancellationToken cancellationToken)
        {
            string returnTo = "";

            string path = $"{_webHostEnvironment.WebRootPath}/sitemap.xml"; //model.WebRootPath + "/content/uploads/sitemap.xml";

            string start = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
            string end = "</urlset>";
            string urls = "";
            int ch = 0;

            if (File.Exists(path))
                File.Delete(path);

            File.AppendAllText(path, start, Encoding.UTF8);

            var newsList = _context.News.Where(n => n.IsPublish).OrderByDescending(n => n.Date).ToList();

            foreach(var n in newsList)
            {
                urls += "<url><loc>https://neo-robots.ru/news/" + n.Path + "</loc><lastmod>" + (n.Date ?? DateTime.Now).ToString("yyyy-MM-ddThh:mm:ss+00:00") + "</lastmod></url>";
            }

            File.AppendAllText(path, urls, Encoding.UTF8);
            File.AppendAllText(path, end, Encoding.UTF8);

            return Task.CompletedTask;
        }
    }
}
