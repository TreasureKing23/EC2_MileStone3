using _1903966_Milestone2.Models;

namespace _1903966_Milestone2.ViewModels
{
    public class ShoeBrandViewModel : CreateShoeBrandViewModel
    {
        public ShoeBrandViewModel()
        {

        }

        public ShoeBrandViewModel(ShoeBrand x)
        {
            this.Id = x.Id;
            this.ShoeId = x.ShoeId;
            this.BrandId = x.BrandId;
        }

        public int Id { get; set; }
    }


    public class CreateShoeBrandViewModel
    {
        public int ShoeId { get; set; }
        public int BrandId { get; set; }


        public ShoeBrand ConvertViewModelToModel(CreateShoeBrandViewModel model)
        {
            return new ShoeBrand
            {
                ShoeId = model.ShoeId,
                BrandId = model.BrandId
            };
        }

        public List<ShoeBrandViewModel> ConvertModelToViewModelList(List<ShoeBrand> modelList)
        {
            return modelList.Select(x => new ShoeBrandViewModel(x)).ToList();
        }
    }
}