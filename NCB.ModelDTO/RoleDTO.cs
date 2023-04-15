using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NCB.ModelDTO
{



    public class RoleDTO : CreateRoleDTO
    {
        [Display(Name = "Role Id")]

        public string Id { get; set; }
    }


    public class CreateRoleDTO
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Needed")]

        public string? Name { get; set; }

        public List<UserDTO>? Users { get; set; }
    }

}
