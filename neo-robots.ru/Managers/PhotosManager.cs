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
    public interface IPhotosManager
    {
        PhotosList GetList(PhotosList m);
        PhotosList ListData(PhotosList m);
        PhotoEdit New();
        PhotoEdit Get(int id);
        PhotoEdit EditorData(PhotoEdit m);
        PhotoEdit Save(PhotoEdit m);
        bool Delete(int id);
        List<Photo> GetCache(int Timer = 600);
    }

    public class PhotosManager : IPhotosManager
    {
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public PhotosManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public PhotoEdit New()
        {
            return new PhotoEdit();
        }
        public PhotoEdit Get(int id)
        {
            return _mapper.Map<PhotoEdit>(_context.Photos.FirstOrDefault(c => c.Id == id));
        }
        public PhotoEdit EditorData(PhotoEdit m)
        {
          
            return m;
        }
        public PhotosList GetList(PhotosList m)
        {
            var res = _context.Photos
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

        public PhotosList ListData(PhotosList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public PhotoEdit Save(PhotoEdit m)
        {
            var Photo = _context.Photos.FirstOrDefault(c => c.Id == m.Id);
            if (Photo == null)
                Photo = new Photo();

            Photo.Name = m.Name;
            Photo.FileName = m.FileName;

            _context.Update(Photo);
            _context.SaveChanges();

            m.Id = Photo.Id;

            SetCache();

            return m;
        }

        public bool Delete(int id)
        {
            var m = _context.Photos.FirstOrDefault(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            _context.SaveChanges();
            return true;
        }

        #region Cache
        private readonly string CacheName = "PhotosManager.List";
        private readonly string CacheNameTimer = "PhotosManager.List.Timer";
        private static readonly object CacheLock = new object();
        public void SetCache(int Timer = 600)
        {
            lock (CacheLock)
            {
                SetCache(_context.Photos.OrderBy(o => o.Name).AsNoTracking().ToList(), Timer);
            }
        }

        public List<Photo> GetCache(int Timer = 600)
        {
            List<Photo> List = new List<Photo>();
            lock (CacheLock)
            {
                if (!_cache.TryGetValue(CacheName, out List))
                {
                    List = _context.Photos.OrderBy(o => o.Name).ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<Photo> List, int Timer = 600)
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
