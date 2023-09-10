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
    public interface IHashTagsManager
    {
        HashTagsList GetList(HashTagsList m);
        HashTagsList ListData(HashTagsList m);
        HashTagEdit New();
        HashTagEdit Get(int id);
        HashTagEdit EditorData(HashTagEdit m);
        HashTagEdit Save(HashTagEdit m);
        bool Delete(int id);
        List<HashTag> GetCache(int Timer = 600);
    }

    public class HashTagsManager : IHashTagsManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public HashTagsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public HashTagEdit New()
        {
            return new HashTagEdit();
        }
        public HashTagEdit Get(int id)
        {
            return _mapper.Map<HashTagEdit>(_context.HashTags.FirstOrDefault(c => c.Id == id));
        }
        public HashTagEdit EditorData(HashTagEdit m)
        {
            return m;
        }
        public HashTagsList GetList(HashTagsList m)
        {
            var res = _context.HashTags
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
                //.Include(i => i.HashTagsCollection)
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToList();

            return m;
        }

        public HashTagsList ListData(HashTagsList m)
        {
            return m;
        }
        public HashTagEdit Save(HashTagEdit m)
        {
            var HashTag = _context.HashTags.FirstOrDefault(c => c.Id == m.Id);
            if (HashTag == null)
                HashTag = new HashTag();

            HashTag.Name = m.Name;

            _context.Update(HashTag);
            _context.SaveChanges();

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.HashTags.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
            return true;
        }

        #region Cache
        private readonly string CacheName = "HashTagsManager.List";
        private readonly string CacheNameTimer = "HashTagsManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.HashTags.OrderBy(o => o.Name).AsNoTracking().ToList(), Timer);
            }
        }

        public List<HashTag> GetCache(int Timer = 600)
        {
            List<HashTag> List = new List<HashTag>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.HashTags.OrderBy(o => o.Name).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<HashTag> List, int Timer = 600)
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
