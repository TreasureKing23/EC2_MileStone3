using _1903966_Milestone2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.ViewModels
{
    public class UserViewModel : CreateUserViewModel
    {
        public string? Id { get; set; }
    }



    public class LoginViewModel
    {
        [Required]
        public string? UserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }


    public class CreateUserViewModel: LoginViewModel
    {
        [Required]

        public string? Name { get; set; }

        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public Gender Gender { get; set; }

        public string? Image { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? Roles { get; set; }

        public IEnumerable<string>? RolesCollection { get; set; }

    }
}
