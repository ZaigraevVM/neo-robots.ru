using Azure;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMI.Managers
{
    public interface IAggregatorManager
    {
        Task ImportNewsListAsync();
        Task ImportNewsAsync();
    }

    public class AggregatorManager : IAggregatorManager
    {
        private IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private IHttpClientFactory _httpClientFactory;
        public AggregatorManager(SmiContext context, IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            Cache = cache;
        }

        public async Task ImportNewsListAsync()
        {
            var newsLists = _context.AggregatorLists.Where(l => l.IsActive).ToList();
            foreach(var newsList in newsLists)
            {
                var download = new AggregatorDownload() {
                    AggregatorListId = newsList.Id,
                    RequestUrl = newsList.Url
                };

                try
                {
                    var httpClient = _httpClientFactory?.CreateClient();
                    using var client = new HttpClient();
                    using var response = await client.GetAsync(newsList.Url);
                    download.ResponseStatus = response.StatusCode.ToString();
                    download.ResponseHtml= await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    download.IsError= true;
                    download.Error = ex.ToString();
                }

                _context.AggregatorDownloads.Add(download);                

                if(download.IsError)
                {
                    var yesterday = DateTime.Now.AddDays(-1);
                    if(_context.AggregatorDownloads.Count(d => d.IsError && d.CreatedAt > yesterday && d.AggregatorListId == newsList.Id) > 2)
                    {
                        newsList.IsActive = false;
                        _context.AggregatorLists.Update(newsList);
                    }
                }
                _context.SaveChanges();
            }
        }

        public async Task ImportNewsAsync()
        {

        }
    }
}
