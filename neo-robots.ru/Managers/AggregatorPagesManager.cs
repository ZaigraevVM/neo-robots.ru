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
    public interface IAggregatorPagesManager : IManager<AggregatorPagesList, AggregatorPageEdit, AggregatorPage> { }

    public class AggregatorPagesManager : IAggregatorPagesManager, ICache<AggregatorPage>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;

        public AggregatorPagesManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            Cache = cache;
        }

        public AggregatorPageEdit New()
        {
            return new AggregatorPageEdit();
        }
        public async Task<AggregatorPageEdit> GetAsync(int id)
        {
            return _mapper.Map<AggregatorPageEdit>(await _context.AggregatorPages.FirstOrDefaultAsync(c => c.Id == id));
        }

        public async Task<AggregatorPageEdit> EditorDataAsync(AggregatorPageEdit m)
        {
            return m;
        }

        public async Task<AggregatorPagesList> GetListAsync(AggregatorPagesList m)
        {
            var res = _context.AggregatorPages
                .Where(w => (m.Search == "" || w.Title.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = res.Count();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public AggregatorPagesList ListData(AggregatorPagesList m)
        {
            return m;
        }

        public async Task<AggregatorPageEdit> SaveAsync(AggregatorPageEdit m)
        {
            var item = await _context.AggregatorPages.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (item == null)
                item = new AggregatorPage();

            item.Title = m.Title;
            item.AggregatorSourceId = m.AggregatorSourceId;
            item.Type = m.Type;
            item.DateHtmlPath = m.DateHtmlPath;
            item.ImageHtmlPath = m.ImageHtmlPath;
            item.SouceTitleHtmlPath = m.SouceTitleHtmlPath;
            item.SouceUrlHtmlPath = m.SouceUrlHtmlPath;
            item.TextHtmlPath = m.TextHtmlPath;
            item.TitleHtmlPath = m.TitleHtmlPath;
            item.UrlRegex = m.UrlRegex;

            _context.Update(item);
            await _context.SaveChangesAsync();

            (this as ICache<AggregatorPage>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.AggregatorPages.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();
            return true;
        }

        public IList<AggregatorPage> GetCache(int Timer = 600)
        {
            return (this as ICache<AggregatorPage>).GetCache(_context, Timer);
        }
    }
}
