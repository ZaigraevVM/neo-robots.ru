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
    public interface IThemesManager : IManager<ThemesList, ThemeEdit, Theme> { }

    public class ThemesManager : IThemesManager, ICache<Theme>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        public ThemesManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            Cache = cache;
        }

        public ThemeEdit New()
        {
            return new ThemeEdit();
        }
        public async Task<ThemeEdit> GetAsync(int id)
        {
            return _mapper.Map<ThemeEdit>(await _context.Themes.FirstOrDefaultAsync(c => c.Id == id));
        }
        public async Task<ThemeEdit> EditorDataAsync(ThemeEdit m)
        {
            return m;
        }
        public async Task<ThemesList> GetListAsync(ThemesList m)
        {
            var res = _context.Themes
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res.Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public ThemesList ListData(ThemesList m)
        {
            return m;
        }
        public async Task<ThemeEdit> SaveAsync(ThemeEdit m)
        {
            var Theme = await _context.Themes.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (Theme == null)
                Theme = new Theme();
                        
            Theme.Name = m.Name;
            Theme.Sorting = m.Sorting;

            _context.Update(Theme);
            await _context.SaveChangesAsync();

            (this as ICache<Theme>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.Themes.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();

            (this as ICache<Theme>).SetCache(_context);

            return true;
        }

        public IList<Theme> GetCache(int Timer = 600)
        {
            return (this as ICache<Theme>).GetCache(_context, Timer);
        }
    }
}
