using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _1903966_Milestone2.ViewModels
{
    public class RoleViewModel : CreateRoleViewModel
    {


        [Display(Name = "Role Id")]
        public string Id { get; set; }
        //public RoleViewModel()
        //{
        //    Users = new List<UserViewModel>();
        //}
        //public RoleViewModel(IdentityRole model)
        //{
        //    Id = model.Id;
        //    Name = model.Name;
        //    Users = new List<UserViewModel>();
        //}
    }




    public class CreateRoleViewModel
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Needed")]

        public string? Name { get; set; }

        public List<UserViewModel>? Users { get; set; }
    }

}
