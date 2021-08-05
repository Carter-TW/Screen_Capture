using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Screen_Capture.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Capture.Services
{
    public class GenericService<T> : IDataService<T> where T: HotKeys
    {
        private readonly HotKeyDBContextFactory _contextFactory;
        public GenericService(HotKeyDBContextFactory factory)
        {
            _contextFactory = factory;
        }
        public async  Task<T> Create(T entity)
        {
            using ( var context= _contextFactory.CreateDbContext() )
            {
                 
                var createdEntity = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdEntity.Entity;
            }
        }

        public  async Task<bool> Deleted(int id)
        {
            using (var context=_contextFactory.CreateDbContext() )
            {
                
                var  entity =await context.Set<T>().FirstOrDefaultAsync(s => s.HotKeyId == id);
                 context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;

            }
        }

        public  async  Task<T> Get(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entity = await context.Set<T>().FirstOrDefaultAsync(s => s.HotKeyId == id);
                return entity;

            }
          
        }

        public  async  Task<IEnumerable<T>> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var  entities= await context.Set<T>().ToListAsync();
                return entities;
                 
                
            }
        }

        public async  Task<T> Update(int id, T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                entity.HotKeyId = id;
                  context.Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
