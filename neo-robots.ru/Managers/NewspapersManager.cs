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
    public interface INewspapersManager
    {
        NewspapersList GetList(NewspapersList m);
        NewspapersList ListData(NewspapersList m);
        NewspaperEdit New();
        NewspaperEdit Get(int id);
        NewspaperEdit EditorData(NewspaperEdit m);
        NewspaperEdit Save(NewspaperEdit m);
        bool Delete(int id);
        List<Newspaper> GetCache(int Timer = 600);
    }

    public class NewspapersManager : INewspapersManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public NewspapersManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public NewspaperEdit New()
        {
            return new NewspaperEdit();
        }
        public NewspaperEdit Get(int id)
        {
            return _mapper.Map<NewspaperEdit>(_context.Newspapers.FirstOrDefault(c => c.Id == id));
        }
        public NewspaperEdit EditorData(NewspaperEdit m)
        {
           
            return m;
        }
        public NewspapersList GetList(NewspapersList m)
        {
            var res = _context.Newspapers
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)) && (m.Select == 0 || w.Id == m.Select));

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

        public NewspapersList ListData(NewspapersList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public NewspaperEdit Save(NewspaperEdit m)
        {
            var Newspaper = _context.Newspapers.FirstOrDefault(c => c.Id == m.Id);
            if (Newspaper == null)
                Newspaper = new Newspaper();

            Newspaper.Name = m.Name;

            _context.Update(Newspaper);
            _context.SaveChanges();

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.Newspapers.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
            return true;
        }

        #region Cache
        private readonly string CacheName = "NewspapersManager.List";
        private readonly string CacheNameTimer = "NewspapersManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.Newspapers.OrderBy(o => o.Name).AsNoTracking().ToList(), Timer);
            }
        }

        public List<Newspaper> GetCache(int Timer = 600)
        {
            List<Newspaper> List = new List<Newspaper>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.Newspapers.OrderBy(o => o.Name).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<Newspaper> List, int Timer = 600)
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
