using _1903966_Milestone2.Models;
using _1903966_Milestone2.Repositories.Interfaces;
using _1903966_Milestone2.Services.Interfaces;
using _1903966_Milestone2.Utilities;
using _1903966_Milestone2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.Services.Implementations
{
    public class OrderService : IGenericService<OrderViewModel>
    {
        private IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<OrderViewModel> GetAll(int pageNumber, int pageSize) 
        {
            int totalCount = 0;
            List<OrderViewModel> vmList = new List<OrderViewModel>();

            try
            {
                int ExcludeRecords = (pageSize + pageNumber) - pageSize;

                var modelList = _unitOfWork.GenericRepository<Order>().GetAll().Result
                        .Take(pageSize).ToList();

                totalCount = _unitOfWork.GenericRepository<Order>().GetAll().Result.ToList().Count();

                vmList = new OrderViewModel().ConvertModelToViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<OrderViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAll()
        {
            var modelList = _unitOfWork.GenericRepository<Order>().GetAll().Result.ToList();
            var vmList = new OrderViewModel().ConvertModelToViewModelList(modelList);
            return vmList;
        }

        public OrderViewModel GetById(int id)
        {
            var model = _unitOfWork.GenericRepository<Order>().GetById(id);
            return new OrderViewModel(model);
        }

        public void Insert(OrderViewModel model)
        {
            var order = new OrderViewModel().ConvertViewModelToModel(model);
            _unitOfWork.GenericRepository<Order>().Insert(order);
            _unitOfWork.Save();
        }

        public async Task InsertAsync(OrderViewModel model)
        {
            var order = new OrderViewModel().ConvertViewModelToModel(model);
            await _unitOfWork.GenericRepository<Order>().Insert(order);
            await _unitOfWork.Save();
        }

        public void Update(OrderViewModel order)
        {
            var model = new OrderViewModel().ConvertViewModelToModel(order);
            var modelById = _unitOfWork.GenericRepository<Order>().GetById(order.Id);

            modelById.Id = order.Id;

            _unitOfWork.GenericRepository<Order>().Update(modelById);
            _unitOfWork.Save();
        }

        public async Task UpdateAsync(OrderViewModel order)
        {
            var model = new OrderViewModel().ConvertViewModelToModel(order);
            var modelById = _unitOfWork.GenericRepository<Order>().GetById(order.Id);

            modelById.Id = order.Id;

            _unitOfWork.GenericRepository<Order>().Update(modelById);
            await _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            var model = _unitOfWork.GenericRepository<Order>().GetById(id);
            _unitOfWork.GenericRepository<Order>().Delete(model);
            _unitOfWork.Save();
        }

        public async Task DeleteAsync(int id)
        {
            var model = _unitOfWork.GenericRepository<Order>().GetById(id);
            _unitOfWork.GenericRepository<Order>().Delete(model);
            await _unitOfWork.Save();
        }

    }
}
