using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Capture.Services
{
    public interface  IDataService<T>
    {
         Task<T> Create(T entity);
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Deleted(int id);
        Task<T> Update(int id, T entity); 
    }
}
