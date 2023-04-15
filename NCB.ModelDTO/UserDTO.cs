using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.ModelDTO
{


    public class LoginDTO
    {
        [DataType(DataType.EmailAddress)]

        public string? Email { get; set; }


        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password should be 8 characters")]

        public string? Password { get; set; }

    }


    public class UserDTO : LoginDTO
    {

        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password should be 8 characters")]

        public string? ConfirmPassword { get; set; }

        public ICollection<string>? Roles { get; set; }
        public string? SelectedRoles { get; set; }
    }
}
