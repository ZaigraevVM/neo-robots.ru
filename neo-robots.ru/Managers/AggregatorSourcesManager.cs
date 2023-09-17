using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Code.Extensions;
using SMI.Data.Entities;
using SMI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMI.Managers.Core;

namespace SMI.Managers
{
    public interface IAggregatorSourcesManager : IManager<AggregatorSourcesList, AggregatorSourceEdit, AggregatorSource> { }

    public class AggregatorSourcesManager : IAggregatorSourcesManager, ICache<AggregatorSource>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        public AggregatorSourcesManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            Cache = cache;
        }

        public AggregatorSourceEdit New()
        {
            return new AggregatorSourceEdit();
        }

        public async Task<AggregatorSourceEdit> GetAsync(int id)
        {
            var source = await _context.AggregatorSources
                .Include(s => s.AggregatorLists)
                .Include(s => s.AggregatorPages)
                .FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<AggregatorSourceEdit>(source);
        }

        public async Task<AggregatorSourceEdit> EditorDataAsync(AggregatorSourceEdit m)
        {
            return m;
        }

        public async Task<AggregatorSourcesList> GetListAsync(AggregatorSourcesList m)
        {
            var res = _context.AggregatorSources
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = res.Count();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public AggregatorSourcesList ListData(AggregatorSourcesList m)
        {
            return m;
        }

        public async Task<AggregatorSourceEdit> SaveAsync(AggregatorSourceEdit m)
        {
            var aggregatorSource = await _context.AggregatorSources.FirstOrDefaultAsync(c => c.Id == m.Id);

            if (aggregatorSource == null)
                aggregatorSource = new AggregatorSource();

            aggregatorSource.Name = m.Name;
            aggregatorSource.Url = m.Url;

            _context.Update(aggregatorSource);
            await _context.SaveChangesAsync();

            (this as ICache<AggregatorSource>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.AggregatorSources.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();
            return true;
        }

        public IList<AggregatorSource> GetCache(int Timer = 600)
        {
            return (this as ICache<AggregatorSource>).GetCache(_context, Timer);
        }
    }
}
