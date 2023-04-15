using _1903966_Milestone2.Models;
using System.ComponentModel.DataAnnotations;

namespace _1903966_Milestone2.ViewModels
{
    public class BrandViewModel : CreateBrandViewModel
    {
        public BrandViewModel()
        {

        }

        public BrandViewModel(Brand x)
        {
            this.Id = x.Id;
            this.Name = x.Name;
        }

        public int Id { get; set; }
    }


    public class CreateBrandViewModel
    {
        [Required]
        [Display(Name = "Brand Name")]

        public string Name { get; set; }


        public Brand ConvertViewModelToModel(CreateBrandViewModel model)
        {
            return new Brand
            {
                Name = model.Name
            };
        }

        public List<BrandViewModel> ConvertModelToViewModelList(List<Brand> modelList)
        {
            return modelList.Select(x => new BrandViewModel(x)).ToList();
        }
    }
}
