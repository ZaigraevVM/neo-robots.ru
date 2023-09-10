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
    public interface IRegionsManager
    {
        RegionsList GetList(RegionsList m);
        RegionsList ListData(RegionsList m);
        RegionEdit New();
        RegionEdit Get(int id);
        RegionEdit EditorData(RegionEdit m);
        RegionEdit Save(RegionEdit m);
        bool Delete(int id);
        List<Region> GetCache(int Timer = 600);
    }

    public class RegionsManager : IRegionsManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public RegionsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public RegionEdit New()
        {
            return new RegionEdit();
        }
        public RegionEdit Get(int id)
        {
            return _mapper.Map<RegionEdit>(_context.Regions.FirstOrDefault(c => c.Id == id));
        }
        public RegionEdit EditorData(RegionEdit m)
        {
            return m;
        }
        public RegionsList GetList(RegionsList m)
        {
            var res = _context.Regions
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

        public RegionsList ListData(RegionsList m)
        {
            return m;
        }
        public RegionEdit Save(RegionEdit m)
        {
            var Region = _context.Regions.FirstOrDefault(c => c.Id == m.Id);
            if (Region == null)
                Region = new Region();

            
            Region.Name = m.Name;

            _context.Update(Region);
            _context.SaveChanges();

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.Regions.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();

            SetCache();

            return true;
        }

        #region Cache
        private readonly string CacheName = "RegionsManager.List";
        private readonly string CacheNameTimer = "RegionsManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.Regions.OrderBy(o => o.Name).AsNoTracking().ToList(), Timer);
            }
        }

        public List<Region> GetCache(int Timer = 600)
        {
            List<Region> List = new List<Region>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.Regions.OrderBy(o => o.Name).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<Region> List, int Timer = 600)
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
