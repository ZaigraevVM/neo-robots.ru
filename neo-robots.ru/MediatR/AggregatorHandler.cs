using AngleSharp;
using AngleSharp.Dom;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SMI.Data.Entities;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SMI.MediatR
{
    public class AggregatorHandler : IRequestHandler<AggregatorRequest>
    {
        private readonly SmiContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AggregatorHandler(SmiContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task Handle(AggregatorRequest request, CancellationToken cancellationToken)
        {
            /*
            IConfiguration config = Configuration.Default;
            var address = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);
            var cellSelector = "tr.vevent td:nth-child(3)";
            var cells = document.QuerySelectorAll(cellSelector);
            var titles = cells.Select(m => m.TextContent);
            */
            /*
            string path = $"{_webHostEnvironment.WebRootPath}/html/ria_list.html";
            var html = await File.ReadAllTextAsync(path);

            var config = AngleSharp.Configuration.Default;
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = await context.OpenAsync(req => req.Content(html));
            var cellSelector = ".content .rubric-list .list.list-tags .list-item .list-item__content a";
            var cells = document.QuerySelectorAll(cellSelector);

            //var titles = cells.Select(m => m.TextContent).ToArray();
            var titles = cells.Select(m => m.Attributes.FirstOrDefault(a=>a.Name == "href")).ToArray();
            */
            /*
            var a = new AggregatorNews();
            a.AggregatorSourceId = 1;
            a.Title = "title";
            a.Text= "text";
            a.Url= "url";
            a.SourceDate = System.DateTime.Now;
            a.ImageUrl = "imageurl";
            a.Html = "html";
            a.AggregatorSourceId = 1;

            _context.AggregatorNews.Add(a);


            var a1 = new AggregatorNews();
            a1.AggregatorSourceId = 1;
            a1.Title = "title";
            a1.Text = "text";
            a1.Url = "url";
            a1.SourceDate = System.DateTime.Now;
            a1.ImageUrl = "imageurl";
            a1.Html = "html";
            a1.AggregatorSourceId = 1;
            a1.NewsId = 1;

            _context.AggregatorNews.Add(a1);
            */

            /*
            var a2 = new AggregatorNews();
            a2.AggregatorSourceId = 1;
            a2.Title = "title";
            a2.Text = "text";
            a2.Url = "url";
            a2.SourceDate = System.DateTime.Now;
            a2.ImageUrl = "imageurl";
            a2.Html = "html";
            a2.AggregatorSourceId = 1;
            a2.News = new News()
            {
                Title= "title",
                Text= "text",
                IsPublish=false
            };

            _context.AggregatorNews.Add(a2);
            */

            var a2 = new AggregatorNews();
            a2.AggregatorSourceId = 1;
            a2.Title = "title";
            a2.Text = "text";
            a2.Url = "url";
            a2.SourceDate = System.DateTime.Now;
            a2.ImageUrl = "imageurl";
            a2.Html = "html";
            a2.AggregatorSourceId = 1;

            var news = new News()
            {
                Title = "title 1 !",
                Text = "text 1 !",
                IsPublish = false,
                AggregatorNews= a2,
            };


            _context.News.Add(news);

            await _context.SaveChangesAsync();

            //return Task.CompletedTask;
        }
    }
}
