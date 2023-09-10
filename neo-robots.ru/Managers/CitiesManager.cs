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
    public interface ICitiesManager
    {
        CitiesList GetList(CitiesList m);
        CitiesList ListData(CitiesList m);
        CityEdit New();
        CityEdit Get(int id);
        CityEdit EditorData(CityEdit m);
        CityEdit Save(CityEdit m);
        bool Delete(int id);
        List<City> GetCache(int Timer = 600);
    }
    
    public class CitiesManager : ICitiesManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRegionsManager _regions;
        public CitiesManager(SmiContext context, IMapper mapper, IMemoryCache cache, IRegionsManager regions)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _regions = regions;
        }

        public CityEdit New()
        {
            return new CityEdit();
        }
        public CityEdit Get(int id)
        {
            return _mapper.Map<CityEdit>(_context.Cities.FirstOrDefault(c => c.Id == id));
        }
        public CityEdit EditorData(CityEdit m)
        {
            m.RegionsList = new List<Region>()
            {
                new Region() { Id = 0, Name = " - "}
            };
            m.RegionsList.AddRange(_regions.GetCache().OrderBy(o => o.Name).ToList());

            return m;
        }
        public CitiesList GetList(CitiesList m)
        {
            var res = _context.Cities
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)) && (m.Select == 0 || w.RegionId == m.Select));

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

            else
            {
                res = res.OrderBy(o => o.Name);
                m.SortField = "name";
                m.SortOrder = "asc";
            }

            m.Count = res.Count();
            m.Items = res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToList();

            return m;
        }

        public CitiesList ListData(CitiesList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public CityEdit Save(CityEdit m)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == m.Id);
            if (city == null)
                city = new City();

            city.Name = m.Name;
            city.RegionId = m.RegionId;

            _context.Update(city);
            _context.SaveChanges();

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
            return true;
        }

        #region Cache
        private readonly string CacheName = "CitiesManager.List";
        private readonly string CacheNameTimer = "CitiesManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.Cities.OrderBy(o => o.Name).AsNoTracking().ToList(), Timer);
            }
        }

        public List<City> GetCache(int Timer = 600)
        {
            List<City> List = new List<City>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.Cities.OrderBy(o => o.Name).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<City> List, int Timer = 600)
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
