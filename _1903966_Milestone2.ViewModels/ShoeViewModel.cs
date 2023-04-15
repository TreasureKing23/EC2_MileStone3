using _1903966_Milestone2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace _1903966_Milestone2.ViewModels
{
    public class ShoeViewModel : CreateShoeViewModel
    {
        public ShoeViewModel()
        {

        }

        public ShoeViewModel(Shoe x)
        {
            this.Id = x.Id;
            this.ShoeModel = x.ShoeModel;
            this.ManufactureDate = x.ManufactureDate;
            this.Quantity = x.Quantity;
            this.Price = x.Price;
            this.Image = x.Image;
        }

        public int Id { get; set; }
    }


    public class CreateShoeViewModel
    {
        public int Id { get; set; }


        [Required]
        [StringLength(100)]
        [Display(Name = "Model")]
        public string ShoeModel { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = "DOB Required")]
        public DateTime ManufactureDate { get; set; }

        [Required]
        [Range(0, 50)]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Image { get; set; }

        public IFormFile ImageFile { get; set; }

        public string? Brands { get; set; }


        [Display(Name = "Brands")]
        public List<int>? BrandSelectedList { get; set; }

        [Display(Name = "Brands")]
        public MultiSelectList? ListOfBrands { get; set; }

        public Shoe ConvertViewModelToModel(CreateShoeViewModel model)
        {
            return new Shoe
            {
                ShoeModel = model.ShoeModel,
                ManufactureDate = model.ManufactureDate,
                Quantity = model.Quantity,
                Price = model.Price,
                Image = model.Image
            };
        }

        public List<ShoeViewModel> ConvertModelToViewModelList(List<Shoe> modelList)
        {
            return modelList.Select(x => new ShoeViewModel(x)).ToList();
        }
    }
}
