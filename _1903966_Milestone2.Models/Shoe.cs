using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _1903966_Milestone2.Models
{
    public class Shoe
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[StringLength(100)]
        //[Display(Name = "Brand")]
        //public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "ShoeModel")]
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
    }
}
