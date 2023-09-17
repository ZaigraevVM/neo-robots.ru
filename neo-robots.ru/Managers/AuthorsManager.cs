using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using SMI.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using SMI.Code.Extensions;
using SMI.Managers.Core;
using System.Threading.Tasks;

namespace SMI.Managers
{
    public interface IAuthorsManager : IManager<AuthorsList, AuthorEdit, Author> { }

    public class AuthorsManager : IAuthorsManager, ICache<Author>
    {
        public IMemoryCache Cache { get; set; }
        public SmiContext _context { get; set; }

        private readonly IMapper _mapper;
        public AuthorsManager(SmiContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            Cache = cache;
        }
        public AuthorEdit New()
        {
            return new AuthorEdit();
        }
        public async Task<AuthorEdit> GetAsync(int id)
        {
            return _mapper.Map<AuthorEdit>(await _context.Authors.FirstOrDefaultAsync(c => c.Id == id));
        }
        public async Task<AuthorEdit> EditorDataAsync(AuthorEdit m)
        {
            return m;
        }
        public async Task<AuthorsList> GetListAsync(AuthorsList m)
        {
            var res = _context.Authors
                .Where(w => (m.Search == "" || w.LastName.Contains(m.Search)))
                .OrderBy(m.SortField, m.SortOrder);

            m.Count = await res.CountAsync();
            m.Items = await res
                .Skip((m.PageIndex - 1) * m.PageSize).Take(m.PageSize).ToListAsync();

            return m;
        }
        public AuthorsList ListData(AuthorsList m)
        {
            return m;
        }
        public async Task<AuthorEdit> SaveAsync(AuthorEdit m)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(c => c.Id == m.Id);
            if (author == null)
                author = new Author();

            author.FirstName = m.FirstName;
            author.LastName = m.LastName;

            _context.Update(author);
            await _context.SaveChangesAsync();

            (this as ICache<Author>).SetCache(_context);

            return m;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var m = await _context.Authors.FirstOrDefaultAsync(c => c.Id == id);
            if (m == null)
                return false;
            _context.Remove(m);
            await _context.SaveChangesAsync();
            (this as ICache<Author>).SetCache(_context);
            return true;
        }

        public IList<Author> GetCache(int Timer = 600)
        {
            return (this as ICache<Author>).GetCache(_context, Timer);
        }
    }
}
