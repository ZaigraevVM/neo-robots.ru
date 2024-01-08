using AngleSharp.Dom;
using AngleSharp;
using AngleSharp.Io;
using Azure;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SMI.Code.Extensions;
using SMI.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

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
                    /*
                    var httpClient = _httpClientFactory?.CreateClient();
                    using var client = new HttpClient();
                    using var response = await client.GetAsync(newsList.Url);
                    download.ResponseStatus = response.StatusCode.ToString();
                    download.ResponseHtml= await response.Content.ReadAsStringAsync();
                    */
                    (download.ResponseStatus, download.ResponseHtml) = await DownloadHtmlAsync(newsList.Url);
                }
                catch (Exception ex)
                {
                    download.IsError = true;
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
            var newsLists = _context.AggregatorLists.Where(l => l.IsActive).ToList();
            foreach(var newsList in newsLists)
            {
                var downloadList = _context.AggregatorDownloads
                    .Include(d => d.AggregatorList)
                    .OrderByDescending(d => d.CreatedAt)
                    .FirstOrDefault(l => l.AggregatorListId == newsList.Id && !l.IsError && l.ResponseStatus == "OK");
                if(downloadList != null)
                {
                    IBrowsingContext context = BrowsingContext.New(AngleSharp.Configuration.Default);
                    IDocument document = await context.OpenAsync(req => req.Content(downloadList.ResponseHtml));
                    var cells = document.QuerySelectorAll(newsList.LinkHtmlPath);
                    var newsUrls = cells.Select(m => m.Attributes.FirstOrDefault(a => a.Name == "href")).ToArray();
                    foreach(var url in newsUrls)
                    {
                        var newsUrl = url.Value.ClearUrl();
                        if (newsUrl.IsValidateUrl())
                        {
                            var download = _context.AggregatorDownloads.FirstOrDefault(n => n.RequestUrl == newsUrl);
                            if(download == null)
                            {
                                var onBreak = false;
                                var pages = _context.AggregatorPages.Where(p => p.AggregatorSourceId == downloadList.AggregatorList.AggregatorSourceId);
                                foreach (var page in pages)
                                {
                                    if (Regex.IsMatch(newsUrl, page.UrlRegex, RegexOptions.IgnoreCase))
                                    {
                                        var downloadPage = new AggregatorDownload()
                                        {
                                            AggregatorPageId = page.Id,
                                            RequestUrl = newsUrl
                                        };
                                        try
                                        {
                                            //var (responseStatus, responseHtml) = await DownloadHtmlAsync(newsUrl);
                                            (downloadPage.ResponseStatus, downloadPage.ResponseHtml) = await DownloadHtmlAsync(newsUrl);

                                        }
                                        catch (Exception ex)
                                        {
                                            downloadPage.IsError = true;
                                            downloadPage.Error = ex.ToString();
                                        }

                                        _context.AggregatorDownloads.Add(downloadPage);

                                        if (downloadPage.IsError)
                                        {
                                            var yesterday = DateTime.Now.AddDays(-1);
                                            if (_context.AggregatorDownloads.Count(d => d.IsError && d.CreatedAt > yesterday && d.AggregatorPageId == page.Id) > 2)
                                            {
                                                newsList.IsActive = false;
                                                _context.AggregatorLists.Update(newsList);
                                            }
                                        }
                                        _context.SaveChanges();
                                        onBreak = true;
                                    }
                                }

                                if(onBreak)
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private async Task<(string status, string html)> DownloadHtmlAsync(string url)
        {
            var httpClient = _httpClientFactory?.CreateClient();
            using var client = new HttpClient();
            using var response = await client.GetAsync(url);
            return (response.StatusCode.ToString(), await response.Content.ReadAsStringAsync());
        }
    }
}
