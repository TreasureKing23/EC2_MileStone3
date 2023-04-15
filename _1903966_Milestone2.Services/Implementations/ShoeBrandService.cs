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
    public class ShoeBrandService : IGenericService<ShoeBrandViewModel>
    {

        private IUnitOfWork _unitOfWork;

        public ShoeBrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<ShoeBrandViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<ShoeBrandViewModel> vmList = new List<ShoeBrandViewModel>();

            try
            {
                int ExcludeRecords = (pageSize + pageNumber) - pageSize;

                var modelList = _unitOfWork.GenericRepository<ShoeBrand>().GetAll().Result
                        .Take(pageSize).ToList();

                totalCount = _unitOfWork.GenericRepository<ShoeBrand>().GetAll().Result.ToList().Count();

                vmList = new ShoeBrandViewModel().ConvertModelToViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<ShoeBrandViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<IEnumerable<ShoeBrandViewModel>> GetAll()
        {
            var modelList = _unitOfWork.GenericRepository<ShoeBrand>().GetAll().Result.ToList();
            var vmList = new ShoeBrandViewModel().ConvertModelToViewModelList(modelList);
            return vmList;
        }

        public ShoeBrandViewModel GetById(int id)
        {
            var model = _unitOfWork.GenericRepository<ShoeBrand>().GetById(id);
            return new ShoeBrandViewModel(model);
        }

        public void Insert(ShoeBrandViewModel model)
        {
            var shoeBrand = new ShoeBrandViewModel().ConvertViewModelToModel(model);
            _unitOfWork.GenericRepository<ShoeBrand>().Insert(shoeBrand);
            _unitOfWork.Save();
        }

        public async Task InsertAsync(ShoeBrandViewModel model)
        {
            var shoeBrand = new ShoeBrandViewModel().ConvertViewModelToModel(model);
           await _unitOfWork.GenericRepository<ShoeBrand>().Insert(shoeBrand);
           await _unitOfWork.Save();
        }

        public void Update(ShoeBrandViewModel shoeBrand)
        {
            var model = new ShoeBrandViewModel().ConvertViewModelToModel(shoeBrand);
            var modelById = _unitOfWork.GenericRepository<ShoeBrand>().GetById(shoeBrand.Id);

            modelById.ShoeId = shoeBrand.ShoeId;
            modelById.BrandId = shoeBrand.BrandId;


            _unitOfWork.GenericRepository<ShoeBrand>().Update(modelById);
            _unitOfWork.Save();
        }

        public async Task UpdateAsync(ShoeBrandViewModel shoeBrand)
        {
            var model = new ShoeBrandViewModel().ConvertViewModelToModel(shoeBrand);
            var modelById = _unitOfWork.GenericRepository<ShoeBrand>().GetById(shoeBrand.Id);

            modelById.ShoeId = shoeBrand.ShoeId;
            modelById.BrandId = shoeBrand.BrandId;

            _unitOfWork.GenericRepository<ShoeBrand>().Update(modelById);
            await _unitOfWork.Save();
        }

        public void Delete(int id) 
        {
            var model = _unitOfWork.GenericRepository<ShoeBrand>().GetById(id);
            _unitOfWork.GenericRepository<ShoeBrand>().Delete(model);
            _unitOfWork.Save();
        }

        public async Task DeleteAsync(int id)
        {
            var model = _unitOfWork.GenericRepository<ShoeBrand>().GetById(id);
            _unitOfWork.GenericRepository<ShoeBrand>().Delete(model);
            await _unitOfWork.Save();
        }

        //private List<ShoeBrandViewModel> ConvertModelToViewModelList(List<ShoeBrand> modelList)
        //{
        //    return modelList.Select(x=> new ShoeBrandViewModel(x)).ToList();
        //}
    }
}
