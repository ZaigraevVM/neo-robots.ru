using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using SMI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SMI.Models;
using Microsoft.Extensions.Logging;
using SMI.Code.Extensions;
using SMI.Managers.Core;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace SMI.Managers
{
    public interface INewsManager : IManager<NewsList, NewsEdit, News>
    {
        HomePage GetHomePage(HomePage m);
        NewsPage GetNewsPage(NewsPage m);
        ThemePage GetThemePage(ThemePage m);
    }

    public class NewsManager : INewsManager, ICache<News>
    {
        public IMemoryCache Cache { get; set; }
        private readonly IMapper _mapper;
        private readonly IAuthorsManager _authors;
        private readonly IPhotosManager _photos;
        private readonly INewspapersManager _newspapers;
        private readonly IRegionsManager _regions;
        private readonly ICitiesManager _cities;
        private readonly IThemesManager _themes;
        private readonly IHashTagsManager _hashtags;
        private readonly ILogger<NewsManager> _logger;
        private readonly SmiContext _context;
        public NewsManager(SmiContext context, IMapper mapper, IMemoryCache cache, IAuthorsManager authors, IPhotosManager photos, INewspapersManager newspapers,
            IRegionsManager regions, ICitiesManager cities, IThemesManager themes, IHashTagsManager hashtags, ILogger<NewsManager> logger)
        {
            Cache = cache;
            _context = context;
            _mapper = mapper;
            _authors = authors;
            _photos = photos;
            _newspapers = newspapers;
            _regions = regions;
            _cities = cities;
            _themes = themes;
            _hashtags = hashtags;
            _logger = logger;
        }

        public NewsEdit New()
        {
            return new NewsEdit();
        }
        public async Task<NewsEdit> GetAsync(int id)
        {
            var news = await _context.News
                .Include(n => n.Photo)
                .Include(n => n.HashTagsNews)
                .Include(n => n.NewsThemes)
                .Include(n => n.NewsRegions)
                .Include(n => n.NewsCities)
                .FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<NewsEdit>(news);
        }
        public async Task<NewsEdit> EditorDataAsync(NewsEdit m)
        {
            m.Authors = new List<Author>()
            {
                new Author() { Id = 0, LastName = " - ", FirstName = " - "}
            };
            m.Authors.AddRange(_authors.GetCache().OrderBy(o => o.LastName).ToList());

            m.Photos = new List<Photo>()
            {
                new Photo() { Id = 0, Name = " - "}
            };
            m.Photos.AddRange(_photos.GetCache().OrderBy(o => o.Name).ToList());

            m.Newspapers = new List<Newspaper>()
            {
                new Newspaper() { Id = 0, Name = " - "}
            };
            m.Newspapers.AddRange(_newspapers.GetCache().OrderBy(o => o.Name).ToList());

            m.Regions = _regions.GetCache().OrderBy(r => r.Name).ToList();
            m.Cities = _cities.GetCache().OrderBy(r => r.Name).ToList();
            m.Themes = _themes.GetCache().OrderBy(r => r.Name).ToList();
            m.HashTags = _hashtags.GetCache().OrderBy(r => r.Name).ToList();

            return m;
        }
        public async Task<NewsList> GetListAsync(NewsList m)
        {
            var res = _context.News
                .Where(w => (m.Search == "" || w.Title.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public NewsList ListData(NewsList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }

        public async Task<NewsEdit> SaveAsync(NewsEdit m)
        {
            m.Path = GetPath(m.Title, m.Id);
            var news = await _context.News
                .Include(n => n.HashTagsNews)
                .Include(n => n.NewsThemes)
                .Include(n => n.NewsRegions)
                .Include(n => n.NewsCities)
                .FirstOrDefaultAsync(c => c.Id == m.Id);

            if (news == null)
                news = new News();

            news.Title = m.Title;
            news.Date = m.Date;
            news.Intro = m.Intro;
            news.Text = m.Text;
            news.PhotoId = m.PhotoId;
            news.Path = m.Path;
            news.IsPublish = m.IsPublish;
            news.AuthorId = m.AuthorId == 0 ? null : m.AuthorId;
            news.NewspapersId = m.NewspapersId == 0 ? null : m.NewspapersId;

            var hashtagsList = await _context.HashTags.Where(w => m.HashTagsIds.Contains(w.Id)).ToListAsync();
            news.HashTagsNews.Clear();
            foreach (var id in m.HashTagsIds)
            {
                var entity = hashtagsList.FirstOrDefault(f => f.Id == id);
                if (entity != null)
                    news.HashTagsNews.Add(new HashTagsNews() { HashTagId = id });
            }

            var themesList = await _context.Themes.Where(w => m.ThemesIds.Contains(w.Id)).ToListAsync();
            news.NewsThemes.Clear();
            foreach (var id in m.ThemesIds)
            {
                var entity = themesList.FirstOrDefault(f => f.Id == id);
                if (entity != null)
                    news.NewsThemes.Add(new NewsTheme() { ThemeId = id });
            }

            var regionsList = await _context.Regions.Where(w => m.RegionsIds.Contains(w.Id)).ToListAsync();
            news.NewsRegions.Clear();
            foreach (var id in m.RegionsIds)
            {
                var entity = regionsList.FirstOrDefault(f => f.Id == id);
                if (entity != null)
                    news.NewsRegions.Add(new NewsRegion() { RegionId = id });
            }

            var citiesList = await _context.Cities.Where(w => m.CitiesIds.Contains(w.Id)).ToListAsync();
            news.NewsCities.Clear();
            foreach (var id in m.CitiesIds)
            {
                var entity = citiesList.FirstOrDefault(f => f.Id == id);
                if (entity != null)
                    news.NewsCities.Add(new NewsCities() { CityId = id });
            }

            _context.Update(news);
            await _context.SaveChangesAsync();
            
            m.Id = news.Id;

            (this as ICache<News>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.News
                .Include(n => n.HashTagsNews)
                .Include(n => n.NewsThemes)
                .Include(n => n.NewsRegions)
                .Include(n => n.NewsCities)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();
            return true;
        }

        public HomePage GetHomePage(HomePage m)
        {
            var homePage = new HomePage();
            var ids = new List<int>();
            homePage.NewsSection1News = _context.News
                .Include(n => n.Photo)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 5))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(3)
                .ToList();

            ids.AddRange(homePage.NewsSection1News.Select(n => n.Id));
                
           homePage.NewsSection2News = _context.News
                .Include(n => n.Photo)
                .Include(n => n.NewsThemes)
                .ThenInclude(nt => nt.Theme)
                .Include(n => n.Newspapers)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n=>n.ThemeId == 2))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(1)
                .ToList();

            ids.AddRange(homePage.NewsSection2News.Select(n => n.Id));

            homePage.NewsSection3News = _context.News
                .Include(n => n.Photo)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 2))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(4)
                .ToList();

            ids.AddRange(homePage.NewsSection3News.Select(n => n.Id));


            homePage.NewsSection4News = _context.News
                .Include(n => n.Photo)
                  .Include(n => n.NewsThemes)
                .ThenInclude(nt => nt.Theme)
                .Include(n => n.Newspapers)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 3))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(1)
                .ToList();
            ids.AddRange(homePage.NewsSection4News.Select(n => n.Id));


            homePage.NewsSection5News = _context.News
               .Include(n => n.Photo)
               .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 3))
               .OrderByDescending(n => n.Date)
               .ThenByDescending(n => n.Id)
               .Take(4)
               .ToList();
            ids.AddRange(homePage.NewsSection5News.Select(n => n.Id));

            homePage.NewsSection6News = _context.News
               .Include(n => n.Photo)
               .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 7))
               .OrderByDescending(n => n.Date)
               .ThenByDescending(n => n.Id)
               .Take(3)
               .ToList();
            ids.AddRange(homePage.NewsSection6News.Select(n => n.Id));

            homePage.NewsSection7News = _context.News
               .Include(n => n.Photo)
               .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 6))
               .OrderByDescending(n => n.Date)
               .ThenByDescending(n => n.Id)
               .Take(3)
               .ToList();
            ids.AddRange(homePage.NewsSection7News.Select(n => n.Id));

            homePage.NewsSection8News = _context.News
               .Include(n => n.Photo)
               .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 12))
               .OrderByDescending(n => n.Date)
               .ThenByDescending(n => n.Id)
               .Take(3)
               .ToList();
            ids.AddRange(homePage.NewsSection8News.Select(n => n.Id));



            homePage.NewsSection1Slider = _context.News
                .Include(n => n.Photo)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 11))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(3)
                .ToList();
            ids.AddRange(homePage.NewsSection1Slider.Select(n => n.Id));


            homePage.NewsSection2Slider = _context.News
                .Include(n => n.Photo)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 8))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(3)
                .ToList();
            ids.AddRange(homePage.NewsSection2Slider.Select(n => n.Id));

            homePage.NewsSection3Slider = _context.News
                .Include(n => n.Photo)
                .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 4))
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(5)
                .ToList();
            ids.AddRange(homePage.NewsSection3Slider.Select(n => n.Id));

            homePage.NewsSection4Slider = _context.News
               .Include(n => n.Photo)
               .Where(n => !ids.Contains(n.Id) && n.NewsThemes.Any(n => n.ThemeId == 9))
               .OrderByDescending(n => n.Date)
               .ThenByDescending(n => n.Id)
               .Take(4 )
               .ToList();
            ids.AddRange(homePage.NewsSection4Slider.Select(n => n.Id));


            return homePage;
        }

        public NewsPage GetNewsPage(NewsPage m)
        {
            var newsPage = new NewsPage();
            newsPage.News = _context.News
                .Include(n => n.Photo)
                .Include(n => n.NewsThemes)
                .ThenInclude(nt => nt.Theme)
                .Include(n => n.Newspapers)
                .First(n => n.Id == m.Id);

            newsPage.NewsSection1News = _context.News
                .Include(n => n.Photo)
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n=>n.Id)
                .Take(10)
                .ToList();

            return newsPage;
        }

        public ThemePage GetThemePage(ThemePage m)
        {
            var themePage = new ThemePage();
            themePage.News = _context.News
                .Include(n => n.Photo)
                .Include(n => n.NewsThemes)
                .ThenInclude(nt => nt.Theme)
                .Include(n => n.Newspapers)
                .First(n => n.Id == m.Id);

            themePage.NewsSection1News = _context.News
                .Include(n => n.Photo)
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .Take(10)
                .ToList();

            return themePage;
        }

        private string GetPath(string title, int id)
        {
            string aliasBase = title.ToPath();
            string aliasReturn = aliasBase;
            bool run = true;
            int count = 0;
            while (run)
            {
                run = false;
                count++;
                var news = _context.News.FirstOrDefault(f => f.Path == aliasReturn && f.Id != id);
                if (news != null && news.Id != id)
                {
                    run = true;
                    aliasReturn = aliasBase + "-" + count;
                }
            }

            return aliasReturn;
        }

        public IList<News> GetCache(int Timer = 600)
        {
            return (this as ICache<News>).GetCache(_context, Timer);
        }
    }
}
