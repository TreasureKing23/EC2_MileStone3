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
    public class ShoeService : IGenericService<ShoeViewModel>
    {

        private IUnitOfWork _unitOfWork;

        public ShoeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<ShoeViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<ShoeViewModel> vmList = new List<ShoeViewModel>();

            try
            {
                int ExcludeRecords = (pageSize + pageNumber) - pageSize;

                var modelList = _unitOfWork.GenericRepository<Shoe>().GetAll().Result
                        .Take(pageSize).ToList();

                totalCount = _unitOfWork.GenericRepository<Shoe>().GetAll().Result.ToList().Count();

                vmList = new ShoeViewModel().ConvertModelToViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<ShoeViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<IEnumerable<ShoeViewModel>> GetAll()
        {
            var modelList = _unitOfWork.GenericRepository<Shoe>().GetAll().Result.ToList();
            var vmList = new ShoeViewModel().ConvertModelToViewModelList(modelList);
            return vmList;
        }

        public ShoeViewModel GetById(int id)
        {
            var model = _unitOfWork.GenericRepository<Shoe>().GetById(id);
            return new ShoeViewModel(model);
        }

        public void Insert(ShoeViewModel model)
        {
            var shoe = new ShoeViewModel().ConvertViewModelToModel(model);
            _unitOfWork.GenericRepository<Shoe>().Insert(shoe);
            _unitOfWork.Save();
        }

        public async Task InsertAsync(ShoeViewModel model)
        {
            var shoe = new ShoeViewModel().ConvertViewModelToModel(model);
           await _unitOfWork.GenericRepository<Shoe>().Insert(shoe);
           await _unitOfWork.Save();
        }

        public void Update(ShoeViewModel shoe)
        {
            var model = new ShoeViewModel().ConvertViewModelToModel(shoe);
            var modelById = _unitOfWork.GenericRepository<Shoe>().GetById(shoe.Id);

            modelById.ShoeModel = model.ShoeModel;
            modelById.ManufactureDate = model.ManufactureDate;
            modelById.Quantity = model.Quantity;
            modelById.Price = model.Price;
            modelById.Image = model.Image;

            _unitOfWork.GenericRepository<Shoe>().Update(modelById);
            _unitOfWork.Save();
        }

        public async Task UpdateAsync(ShoeViewModel shoe)
        {
            var model = new ShoeViewModel().ConvertViewModelToModel(shoe);
            var modelById = _unitOfWork.GenericRepository<Shoe>().GetById(shoe.Id);

            modelById.ShoeModel = shoe.ShoeModel;
            modelById.ManufactureDate = shoe.ManufactureDate;
            modelById.Quantity = shoe.Quantity;
            modelById.Price = shoe.Price;
            modelById.Image = shoe.Image;

            _unitOfWork.GenericRepository<Shoe>().Update(modelById);
            await _unitOfWork.Save();
        }

        public void Delete(int id) 
        {
            var model = _unitOfWork.GenericRepository<Shoe>().GetById(id);
            _unitOfWork.GenericRepository<Shoe>().Delete(model);
            _unitOfWork.Save();
        }

        public async Task DeleteAsync(int id)
        {
            var model = _unitOfWork.GenericRepository<Shoe>().GetById(id);
            _unitOfWork.GenericRepository<Shoe>().Delete(model);
            await _unitOfWork.Save();
        }

        //private List<ShoeViewModel> ConvertModelToViewModelList(List<Shoe> modelList)
        //{
        //    return modelList.Select(x=> new ShoeViewModel(x)).ToList();
        //}
    }
}
