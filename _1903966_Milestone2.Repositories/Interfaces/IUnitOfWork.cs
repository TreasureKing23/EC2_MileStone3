using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<T> GenericRepository<T>() where T : class;
        Task Save();
    }
}
