using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using SMI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SMI.Models;

namespace SMI.Managers
{
    public interface INewsManager
    {
        NewsList GetList(NewsList m);
        NewsList ListData(NewsList m);
        NewsEdit New();
        NewsEdit Get(int id);
        NewsEdit EditorData(NewsEdit m);
        NewsEdit Save(NewsEdit m);
        bool Delete(int id);
        List<News> GetCache(int Timer = 600);
        HomePage GetHomePage(HomePage m);
        NewsPage GetNewsPage(NewsPage m);
    }

    public class NewsManager : INewsManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IAuthorsManager _authors;
        private readonly IPhotosManager _photos;
        private readonly INewspapersManager _newspapers;
        private readonly IRegionsManager _regions;
        private readonly ICitiesManager _cities;
        private readonly IThemesManager _themes;
        private readonly IHashTagsManager _hashtags;
        public NewsManager(SmiContext context, IMapper mapper, IMemoryCache cache, IAuthorsManager authors, IPhotosManager photos, INewspapersManager newspapers,
            IRegionsManager regions, ICitiesManager cities, IThemesManager themes, IHashTagsManager hashtags)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _authors = authors;
            _photos = photos;
            _newspapers = newspapers;
            _regions = regions;
            _cities = cities;
            _themes = themes;
            _hashtags = hashtags;
        }

        public NewsEdit New()
        {
            return new NewsEdit();
        }
        public NewsEdit Get(int id)
        {
            var news = _context.News
                .Include(n => n.Photo)
                .Include(n => n.HashTagsNews)
                .Include(n => n.NewsThemes)
                .FirstOrDefault(c => c.Id == id);
            return _mapper.Map<NewsEdit>(news);
        }
        public NewsEdit EditorData(NewsEdit m)
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

            m.NewsRegions = _regions.GetCache().OrderBy(r => r.Name).ToList();
            m.NewsCities = _cities.GetCache().OrderBy(r => r.Name).ToList();
            m.Themes = _themes.GetCache().OrderBy(r => r.Name).ToList();
            m.HashTags = _hashtags.GetCache().OrderBy(r => r.Name).ToList();

            return m;
        }
        public NewsList GetList(NewsList m)
        {
            var res = _context.News
                .Where(w => (m.Search == "" || w.Title.Contains(m.Search)));

            if ("name" == m.SortField)
            {
                if ("asc" == m.SortOrder)
                    res = res.OrderBy(o => o.Title);
                else
                    res = res.OrderByDescending(o => o.Title);
            }
            else if ("id" == m.SortField)
            {
                if ("asc" == m.SortOrder)
                    res = res.OrderBy(o => o.Id);
                else
                    res = res.OrderByDescending(o => o.Id);
            }

            else
            {
                res = res.OrderBy(o => o.Title);
                m.SortField = "name";
                m.SortOrder = "asc";
            }

            m.Count = res.Count();
            m.Items = res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToList();

            return m;
        }

        public NewsList ListData(NewsList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public NewsEdit Save(NewsEdit m)
        {
            var news = _context.News
                .Include(n => n.HashTagsNews)
                .Include(n => n.NewsThemes)
                .FirstOrDefault(c => c.Id == m.Id);
            if (news == null)
                news = new News();

            news.Title = m.Title;
            news.Date = m.Date;
            news.Text = m.Text;
            news.PhotoId = m.PhotoId;
            
            news.AuthorId = m.AuthorId == 0 ? null : m.AuthorId;
            news.NewspapersId = m.NewspapersId == 0 ? null : m.NewspapersId;

            var hashtagsList = _context.HashTags.Where(w => m.HashTagsIds.Contains(w.Id)).ToList();
            news.HashTagsNews.Clear();
            foreach (var id in m.HashTagsIds)
            {
                var entity = hashtagsList.FirstOrDefault(f => f.Id == id);
                if (entity != null)
                    news.HashTagsNews.Add(new HashTagsNews() { HashTagId = id });
            }

            var themesList = _context.Themes.Where(w => m.ThemesIds.Contains(w.Id)).ToList();
            news.NewsThemes.Clear();
            foreach (var id in m.ThemesIds)
            {
                var entity = themesList.FirstOrDefault(f => f.Id == id);
                if (entity != null)
                    news.NewsThemes.Add(new NewsTheme() { ThemeId = id });
            }

            _context.Update(news);
            _context.SaveChanges();
            
            m.Id = news.Id;

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.News.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
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

        #region Cache
        private readonly string CacheName = "NewsManager.List";
        private readonly string CacheNameTimer = "NewsManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.News.OrderBy(o => o.Id).AsNoTracking().ToList(), Timer);
            }
        }

        public List<News> GetCache(int Timer = 600)
        {
            List<News> List = new List<News>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.News.OrderBy(o => o.Id).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<News> List, int Timer = 600)
        {
            _cache.Set(CacheName, List, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(Timer + 60)
            });
            _cache.Set(CacheNameTimer, DateTime.Now.AddSeconds(Timer), new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(Timer)
            });
        }
        #endregion
    }
}
