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
    public class BrandService : IGenericService<BrandViewModel>
    {

        private IUnitOfWork _unitOfWork;

        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<BrandViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<BrandViewModel> vmList = new List<BrandViewModel>();

            try
            {
                int ExcludeRecords = (pageSize + pageNumber) - pageSize;

                var modelList = _unitOfWork.GenericRepository<Brand>().GetAll().Result
                        .Take(pageSize).ToList();

                totalCount = _unitOfWork.GenericRepository<Brand>().GetAll().Result.ToList().Count();

                vmList = new BrandViewModel().ConvertModelToViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<BrandViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<IEnumerable<BrandViewModel>> GetAll()
        {
            var modelList = _unitOfWork.GenericRepository<Brand>().GetAll().Result.ToList();
            var vmList = new BrandViewModel().ConvertModelToViewModelList(modelList);
            return vmList;
        }

        public BrandViewModel GetById(int id)
        {
            var model = _unitOfWork.GenericRepository<Brand>().GetById(id);
            return new BrandViewModel(model);
        }

        public void Insert(BrandViewModel model)
        {
            var brand = new BrandViewModel().ConvertViewModelToModel(model);
            _unitOfWork.GenericRepository<Brand>().Insert(brand);
            _unitOfWork.Save();
        }

        public async Task InsertAsync(BrandViewModel model)
        {
            var brand = new BrandViewModel().ConvertViewModelToModel(model);
           await _unitOfWork.GenericRepository<Brand>().Insert(brand);
           await _unitOfWork.Save();
        }

        public void Update(BrandViewModel brand)
        {
            var model = new BrandViewModel().ConvertViewModelToModel(brand);
            var modelById = _unitOfWork.GenericRepository<Brand>().GetById(brand.Id);

            modelById.Name = brand.Name;

            _unitOfWork.GenericRepository<Brand>().Update(modelById);
            _unitOfWork.Save();
        }

        public async Task UpdateAsync(BrandViewModel brand)
        {
            var model = new BrandViewModel().ConvertViewModelToModel(brand);
            var modelById = _unitOfWork.GenericRepository<Brand>().GetById(brand.Id);

            modelById.Name = brand.Name;

            _unitOfWork.GenericRepository<Brand>().Update(modelById);
            await _unitOfWork.Save();
        }

        public void Delete(int id) 
        {
            var model = _unitOfWork.GenericRepository<Brand>().GetById(id);
            _unitOfWork.GenericRepository<Brand>().Delete(model);
            _unitOfWork.Save();
        }

        public async Task DeleteAsync(int id)
        {
            var model = _unitOfWork.GenericRepository<Brand>().GetById(id);
            _unitOfWork.GenericRepository<Brand>().Delete(model);
            await _unitOfWork.Save();
        }

        //private List<BrandViewModel> ConvertModelToViewModelList(List<Brand> modelList)
        //{
        //    return modelList.Select(x=> new BrandViewModel(x)).ToList();
        //}
    }
}
