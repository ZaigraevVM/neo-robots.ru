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

namespace SMI.Managers
{
    public interface IHashTagsManager : IManager<HashTagsList, HashTagEdit, HashTag> { }

    public class HashTagsManager : IHashTagsManager, ICache<HashTag>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        public HashTagsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            Cache = cache;
            _context = context;
            _mapper = mapper;
        }

        public HashTagEdit New()
        {
            return new HashTagEdit();
        }
        public async Task<HashTagEdit> GetAsync(int id)
        {
            return _mapper.Map<HashTagEdit>(await _context.HashTags.FirstOrDefaultAsync(c => c.Id == id));
        }
        public async Task<HashTagEdit> EditorDataAsync(HashTagEdit m)
        {
            return m;
        }
        public async Task<HashTagsList> GetListAsync(HashTagsList m)
        {
            var res = _context.HashTags
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res
                //.Include(i => i.HashTagsCollection)
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public HashTagsList ListData(HashTagsList m)
        {
            return m;
        }
        public async Task<HashTagEdit> SaveAsync(HashTagEdit m)
        {
            var HashTag = await _context.HashTags.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (HashTag == null)
                HashTag = new HashTag();

            HashTag.Name = m.Name;

            _context.Update(HashTag);
            await _context.SaveChangesAsync();

            (this as ICache<HashTag>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.HashTags.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();

            (this as ICache<HashTag>).SetCache(_context);

            return true;
        }

        public IList<HashTag> GetCache(int Timer = 600)
        {
            return (this as ICache<HashTag>).GetCache(_context, Timer);
        }
    }
}
