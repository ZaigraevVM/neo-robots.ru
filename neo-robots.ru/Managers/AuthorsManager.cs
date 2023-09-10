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
    public interface IAuthorsManager
    {
        AuthorsList GetList(AuthorsList m);
        AuthorsList ListData(AuthorsList m);
        AuthorEdit New();
        AuthorEdit Get(int id);
        AuthorEdit EditorData(AuthorEdit m);
        AuthorEdit Save(AuthorEdit m);
        bool Delete(int id);
        List<Author> GetCache(int Timer = 600);
    }

    public class AuthorsManager : IAuthorsManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public AuthorsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public AuthorEdit New()
        {
            return new AuthorEdit();
        }
        public AuthorEdit Get(int id)
        {
            return _mapper.Map<AuthorEdit>(_context.Authors.FirstOrDefault(c => c.Id == id));
        }
        public AuthorEdit EditorData(AuthorEdit m)
        {
            return m;
        }
        public AuthorsList GetList(AuthorsList m)
        {
            var res = _context.Authors
                .Where(w => (m.Search == "" || w.LastName.Contains(m.Search)));

            if ("name" == m.SortField)
            {
                if ("asc" == m.SortOrder)
                    res = res.OrderBy(o => o.LastName);
                else
                    res = res.OrderByDescending(o => o.LastName);
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

        public AuthorsList ListData(AuthorsList m)
        {
            return m;
        }
        public AuthorEdit Save(AuthorEdit m)
        {
            var author = _context.Authors.FirstOrDefault(c => c.Id == m.Id);
            if (author == null)
                author = new Author();

            
            author.FirstName = m.FirstName;
            author.LastName = m.LastName;

            _context.Update(author);
            _context.SaveChanges();

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.Authors.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
            return true;
        }

        #region Cache
        private readonly string CacheName = "AuthorsManager.List";
        private readonly string CacheNameTimer = "AuthorsManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.Authors.OrderBy(o => o.FirstName).AsNoTracking().ToList(), Timer);
            }
        }

        public List<Author> GetCache(int Timer = 600)
        {
            List<Author> List = new List<Author>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.Authors.OrderBy(o => o.LastName).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<Author> List, int Timer = 600)
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
