using NCB.Repositories.Data;
using NCB.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;

        private bool disposed = false;

        public UnitOfWork(ApiDbContext appDbContext)
        {
            _context = appDbContext;
        }


        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            IGenericRepository<T> repo = new GenericRepository<T>(_context);
            return repo;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
