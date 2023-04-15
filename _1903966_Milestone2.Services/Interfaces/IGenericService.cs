using _1903966_Milestone2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        PagedResult<T> GetAll(int pageNumber, int pageSize);

        Task<IEnumerable<T>> GetAll();

        T GetById(int id);

        void Update(T model);
        Task UpdateAsync(T model);

        void Insert(T model);
        Task InsertAsync(T model);

        void Delete(int id);
        Task DeleteAsync(int id);


    }
}
