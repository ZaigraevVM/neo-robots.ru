using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SMI.Data.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SMI.Managers.Core
{
    public interface ICache<TEntity> where TEntity : class
    {
        IMemoryCache Cache { get; set; }
        public void SetCache(SmiContext context, int Timer = 600)
        {
            lock (GetName())
            {
                SetCache(context.Set<TEntity>().AsNoTracking().ToList(), Timer);
            }
        }

        public IList<TEntity> GetCache(SmiContext context, int Timer = 600)
        {
            List<TEntity> List = new List<TEntity>();
            lock (GetName())
            {
                if (!Cache.TryGetValue(GetName(), out List))
                {
                    List = context.Set<TEntity>().ToList();
                    SetCache(List, Timer);
                }
            }
            return List;
        }

        private void SetCache(List<TEntity> List, int Timer = 600)
        {
            Cache.Set(GetName(), List, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(Timer + 60)
            });
        }

        private string GetName() => $"{this.GetType().FullName}_cache";
    }
}
