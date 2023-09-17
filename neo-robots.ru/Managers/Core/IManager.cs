using SMI.Areas.Admin.Models;
using SMI.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMI.Managers.Core
{
    public interface IManager<TList, TEdit, T>
    {
        Task<TList> GetListAsync(TList m);
        TList ListData(TList m);
        TEdit New();
        Task<TEdit> GetAsync(int id);
        Task<TEdit> EditorDataAsync(TEdit m);
        Task<TEdit> SaveAsync(TEdit m);
        Task<bool> DeleteAsync(int id);
        IList<T> GetCache(int Timer = 600);
    }
}
