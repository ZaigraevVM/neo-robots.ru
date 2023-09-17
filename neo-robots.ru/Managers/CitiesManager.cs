using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Areas.Admin.Models;
using SMI.Code.Extensions;
using SMI.Data.Entities;
using SMI.Managers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMI.Managers
{
    public interface ICitiesManager : IManager<CitiesList, CityEdit, City>
    {
        Task<IList<City>> FilteredByRegionsAsync(string filter);
    }

    public class CitiesManager : ICitiesManager, ICache<City>
    {
        public IMemoryCache Cache { get; set; }
        private readonly SmiContext _context;
        private readonly IMapper _mapper;
        private readonly IRegionsManager _regions;
        public CitiesManager(SmiContext context, IMapper mapper, IMemoryCache cache, IRegionsManager regions)
        {
            Cache = cache;
            _context = context;
            _mapper = mapper;
            _regions = regions;
        }
        public CityEdit New()
        {
            return new CityEdit();
        }
        public async Task<CityEdit> GetAsync(int id)
        {
            return _mapper.Map<CityEdit>(_context.Cities.FirstOrDefault(c => c.Id == id));
        }
        public async Task<CityEdit> EditorDataAsync(CityEdit m)
        {
            m.RegionsList = new List<Region>()
            {
                new Region() { Id = 0, Name = " - "}
            };
            m.RegionsList.AddRange(_regions.GetCache().OrderBy(o => o.Name).ToList());

            return m;
        }
        public async Task<CitiesList> GetListAsync(CitiesList m)
        {
            var res = _context.Cities
                .Include(c => c.Region)
                .Where(w => (m.Search == "" || w.Name.Contains(m.Search)) && (m.Select == 0 || w.RegionId == m.Select))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }

        public CitiesList ListData(CitiesList m)
        {
            /*m.RegionsList = _region.GetCache();*/
            return m;
        }
        public async Task<CityEdit> SaveAsync(CityEdit m)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (city == null)
                city = new City();

            city.Name = m.Name;
            city.RegionId = m.RegionId;

            _context.Update(city);
            await _context.SaveChangesAsync();

            (this as ICache<City>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();

            (this as ICache<City>).SetCache(_context);

            return true;
        }

        public async Task<IList<City>> FilteredByRegionsAsync(string filter)
        {
            var regions = filter.Replace("[", "").Replace("]", "").Replace("\"", "").Split(',').Select(m => Convert.ToInt32(m)).ToArray();
            return await _context.Cities.Where(c => regions.Contains(c.RegionId)).ToListAsync();
        }

        public IList<City> GetCache(int Timer = 600)
        {
            return (this as ICache<City>).GetCache(_context, Timer);
        }
    }
}
