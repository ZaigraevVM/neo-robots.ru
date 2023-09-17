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
    public interface IRegionsManager : IManager<RegionsList, RegionEdit, Region> { }

    public class RegionsManager : IRegionsManager, ICache<Region>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        public RegionsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            Cache = cache;
            _context = context;
            _mapper = mapper;
        }

        public RegionEdit New()
        {
            return new RegionEdit();
        }
        public async Task<RegionEdit> GetAsync(int id)
        {
            return _mapper.Map<RegionEdit>(_context.Regions.FirstOrDefaultAsync(c => c.Id == id));
        }
        public async Task<RegionEdit> EditorDataAsync(RegionEdit m)
        {
            return m;
        }
        public async Task<RegionsList> GetListAsync(RegionsList m)
        {
            var res = _context.Regions
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res.Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public RegionsList ListData(RegionsList m)
        {
            return m;
        }
        public async Task<RegionEdit> SaveAsync(RegionEdit m)
        {
            var Region = await _context.Regions.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (Region == null)
                Region = new Region();

            
            Region.Name = m.Name;

            _context.Update(Region);
            await _context.SaveChangesAsync();

            (this as ICache<Region>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.Regions.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();

            (this as ICache<Region>).SetCache(_context);

            return true;
        }

        public IList<Region> GetCache(int Timer = 600)
        {
            return (this as ICache<Region>).GetCache(_context, Timer);
        }
    }
}
