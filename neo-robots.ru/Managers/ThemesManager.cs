using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using SMI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMI.Managers
{
    public interface IThemesManager
    {
        ThemesList GetList(ThemesList m);
        ThemesList ListData(ThemesList m);
        ThemeEdit New();
        ThemeEdit Get(int id);
        ThemeEdit EditorData(ThemeEdit m);
        ThemeEdit Save(ThemeEdit m);
        bool Delete(int id);
        List<Theme> GetCache(int Timer = 600);
    }

    public class ThemesManager : IThemesManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public ThemesManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public ThemeEdit New()
        {
            return new ThemeEdit();
        }
        public ThemeEdit Get(int id)
        {
            return _mapper.Map<ThemeEdit>(_context.Themes.FirstOrDefault(c => c.Id == id));
        }
        public ThemeEdit EditorData(ThemeEdit m)
        {
            return m;
        }
        public ThemesList GetList(ThemesList m)
        {
            var res = _context.Themes
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)));

            if ("name" == m.SortField)
            {
                if ("asc" == m.SortOrder)
                    res = res.OrderBy(o => o.Name);
                else
                    res = res.OrderByDescending(o => o.Name);
            }
            else if ("id" == m.SortField)
            {
                if ("asc" == m.SortOrder)
                    res = res.OrderBy(o => o.Id);
                else
                    res = res.OrderByDescending(o => o.Id);
            }

            m.Count = res.Count();
            m.Items = res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToList();

            return m;
        }

        public ThemesList ListData(ThemesList m)
        {
            return m;
        }
        public ThemeEdit Save(ThemeEdit m)
        {
            var Theme = _context.Themes.FirstOrDefault(c => c.Id == m.Id);
            if (Theme == null)
                Theme = new Theme();

            
            Theme.Name = m.Name;

            _context.Update(Theme);
            _context.SaveChanges();

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.Themes.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
            return true;
        }

        #region Cache
        private readonly string CacheName = "ThemesManager.List";
        private readonly string CacheNameTimer = "ThemesManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.Themes.OrderBy(o => o.Name).AsNoTracking().ToList(), Timer);
            }
        }

        public List<Theme> GetCache(int Timer = 600)
        {
            List<Theme> List = new List<Theme>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.Themes.OrderBy(o => o.Name).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<Theme> List, int Timer = 600)
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
