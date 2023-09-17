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
    public interface INewspapersManager : IManager<NewspapersList, NewspaperEdit, Newspaper> { }

    public class NewspapersManager : INewspapersManager, ICache<Newspaper>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        public NewspapersManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            Cache = cache;
        }
        public NewspaperEdit New()
        {
            return new NewspaperEdit();
        }
        public async Task<NewspaperEdit> GetAsync(int id)
        {
            return _mapper.Map<NewspaperEdit>(await _context.Newspapers.FirstOrDefaultAsync(c => c.Id == id));
        }
        public async Task<NewspaperEdit> EditorDataAsync(NewspaperEdit m)
        {
            return m;
        }
        public async Task<NewspapersList> GetListAsync(NewspapersList m)
        {
            var res = _context.Newspapers
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)) && (m.Select == 0 || w.Id == m.Select))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize)
                .Take(m.PageSize)
                .ToListAsync();

            return m;
        }
        public NewspapersList ListData(NewspapersList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public async Task<NewspaperEdit> SaveAsync(NewspaperEdit m)
        {
            var Newspaper = await _context.Newspapers.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (Newspaper == null)
                Newspaper = new Newspaper();

            Newspaper.Name = m.Name;

            _context.Update(Newspaper);
            await _context.SaveChangesAsync();

            (this as ICache<Newspaper>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.Newspapers.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();

            (this as ICache<Newspaper>).SetCache(_context);

            return true;
        }

        public IList<Newspaper> GetCache(int Timer = 600)
        {
            return (this as ICache<Newspaper>).GetCache(_context, Timer);
        }
    }
}
