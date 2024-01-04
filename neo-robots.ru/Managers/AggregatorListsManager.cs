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
    public interface IAggregatorListsManager : IManager<AggregatorListsList, AggregatorListEdit, AggregatorList>
    {
       
    }

    public class AggregatorListsManager : IAggregatorListsManager, ICache<AggregatorList>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        
        public AggregatorListsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            Cache = cache;
        }

        public AggregatorListEdit New()
        {
            return new AggregatorListEdit();
        }
        public async Task<AggregatorListEdit> GetAsync(int id)
        {
            return _mapper.Map<AggregatorListEdit>(await _context.AggregatorLists.FirstOrDefaultAsync(c => c.Id == id));
        }

        public async Task<AggregatorListEdit> EditorDataAsync(AggregatorListEdit m)
        {
            return m;
        }

        public async Task<AggregatorListsList> GetListAsync(AggregatorListsList m)
        {
            var res = _context.AggregatorLists
                .Where(w => (m.Search == "" || w.Title.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = res.Count();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public AggregatorListsList ListData(AggregatorListsList m)
        {
            return m;
        }

        public async Task<AggregatorListEdit> SaveAsync(AggregatorListEdit m)
        {
            var item = await _context.AggregatorLists.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (item == null)
                item = new AggregatorList();

            item.Title = m.Title;
            item.Url = m.Url;
            item.AggregatorSourceId = m.AggregatorSourceId;
            item.LinkHtmlPath = m.LinkHtmlPath;
            item.Type = m.Type;
            item.IsActive = m.IsActive;

            _context.Update(item);
            await _context.SaveChangesAsync();

            (this as ICache<AggregatorList>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.AggregatorLists.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();
            return true;
        }

        public IList<AggregatorList> GetCache(int Timer = 600)
        {
            return (this as ICache<AggregatorList>).GetCache(_context, Timer);
        }
    }
}
