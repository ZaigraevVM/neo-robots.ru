using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using SMI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SMI.Code.Extensions;
using SMI.Managers.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SMI.Managers
{
    public interface IPhotosManager : IManager<PhotosList, PhotoEdit, Photo> { }

    public class PhotosManager : IPhotosManager, ICache<Photo>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        public PhotosManager(SmiContext context, IMapper mapper, IMemoryCache cache, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            Cache = cache;
        }

        public PhotoEdit New()
        {
            return new PhotoEdit();
        }
        public async Task<PhotoEdit> GetAsync(int id)
        {
            return _mapper.Map<PhotoEdit>(await _context.Photos.FirstOrDefaultAsync(c => c.Id == id));
        }
        public async Task<PhotoEdit> EditorDataAsync(PhotoEdit m)
        {
            return m;
        }
        public async Task<PhotosList> GetListAsync(PhotosList m)
        {
            var res = _context.Photos
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res.Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public PhotosList ListData(PhotosList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public async Task<PhotoEdit> SaveAsync(PhotoEdit m)
        {
            var item = await _context.Photos.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (item == null)
                item = new Photo();

            item.Name = m.Name;
            item.FileName = m.FileName;
            item.SourceUrl = m.SourceUrl;

            _context.Update(item);
            await _context.SaveChangesAsync();

            m.Id = item.Id;

            (this as ICache<Photo>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.Photos.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;

            string webRootPath = _hostingEnvironment.WebRootPath;
            File.Delete(webRootPath + "/images/news/originals/" + m.FileName);
            File.Delete(webRootPath + "/images/news/w1000x523/" + m.FileName);
            File.Delete(webRootPath + "/images/news/w100x100/" + m.FileName);
            File.Delete(webRootPath + "/images/news/w200x150/" + m.FileName);
            File.Delete(webRootPath + "/images/news/w450x150/" + m.FileName);
            File.Delete(webRootPath + "/images/news/w500x300/" + m.FileName);
            File.Delete(webRootPath + "/images/news/w890x534/" + m.FileName);

            _context.Remove(m);
            await _context.SaveChangesAsync();
            (this as ICache<Photo>).SetCache(_context);
            return true;
        }

        public IList<Photo> GetCache(int Timer = 600)
        {
            return (this as ICache<Photo>).GetCache(_context, Timer);
        }
    }
}
