using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NCB.ModelDTO;
using NCB.Services.Interfaces;
using System.Xml.Linq;

namespace NCB.Web.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IAuthManager _authManager;

        public AdministrationController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDTO model)
        {
            if (model.Name != null)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.Name
                };

                IdentityResult res = await _authManager.CreateRoleAsync(identityRole);
                if (res.Succeeded)
                {
                    TempData["AlertMsg"] = "Role Created!";
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _authManager.FindRoleByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {Id} cannor be found";
                return View("NotFound");
            }


            var model = new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
            var users = await _authManager.GetAllUsersAsync();
            model.Users = new List<UserDTO>();
            foreach (var user in users)
            {
                if (await _authManager.IsInRoleAsync(user, role.Name))
                    model.Users.Add(user);
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditRole(RoleDTO model)
        {
            if (ModelState.IsValid)
            {
                bool roleNameExists = await _authManager.RoleExistsAsync(model.Name);
                if (!roleNameExists)
                {
                    var role = await _authManager.FindRoleByIdAsync(model.Id);
                    if (role != null)
                    {
                        role.Name = model.Name;
                        IdentityResult res = await _authManager.UpdateRoleAsync(role);
                        if (res.Succeeded)
                        {
                            TempData["AlertMsg"] = "Role created!";
                            return RedirectToAction("ListRoles", "Administration");
                        }
                        foreach (var error in res.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    ModelState.AddModelError("", "Role Id Does Not Exists in Database");

                }
                ModelState.AddModelError("", "Role Already Exists in Database");
            }
            return View(model);
        }




        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _authManager.GetAllRolesAsync();
            return View(roles);
        }

        [HttpGet]

        public async Task<IActionResult> ListUsers()
        {
            var users = await _authManager.GetAllUsersAsync();
            foreach (var item in users)
            {
                var user = await _authManager.FindUserByIdAsync(item.Id);
                IList<string> userRoles = await _authManager.GetUserRolesAsync(user);
                item.SelectedRoles = String.Join(", ", userRoles.ToList());
                //item. Roles = userRoles;
            }

            return View(users);
        }

        //[HttpGet]
        //public async Task<IActionResult> ListTransactions()
        //{
        //    var trans = _authManager.GetAllTransactionsAsync();
        //    return View(trans);
        //}


        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO model)
        {

            if (!ModelState.IsValid)
                return View(model);
            var result = await _authManager.RegisterAsync(model);
            if (result.StatusCode == 0)
            {
                TempData["msg"] = result.StatusMessage;
                return View(model);
            }
            TempData["AlertMsg"] = "User created!";
            return RedirectToAction("ListUsers");
        }




        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var UserDTO = await _authManager.FindUserByIdAsync(id);
            if (UserDTO == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} cannor be found";
                return View("NotFound");
            }

            IList<string> userRoles = await _authManager.GetUserRolesAsync(UserDTO);
            UserDTO.Roles = userRoles.ToList();
            ViewBag.Roles = _authManager.GetAllRolesAsync().ToList();
            return View(UserDTO);
        }



        [HttpPost]

        public async Task<IActionResult> EditUser(UserDTO model)
        {
            var result = await _authManager.UpdateUserAsync(model);
            TempData["AlertMsg"] = "User Updated!";
            return RedirectToAction("ListUsers");
        }




        [HttpPost]
        public async Task<IActionResult> UpdateUserRoles(UserDTO model)
        {
            var user = await _authManager.FindUserByIdAsync(model.Id);
            if (user != null)
            {
                //remove all roles
                var userRoles = await _authManager.GetUserRolesAsync(user);
                IdentityResult result = await _authManager.RemoveUserFromRolesAsync(user, userRoles.ToList());
                if (model.Roles.Count > 0)
                {

                    //add user to roles
                    result = await _authManager.AddUserToRolesAsync(user, model.Roles.ToArray());
                }
            }
            TempData["AlertMsg"] = "User Roles Updated!";
            return RedirectToAction("EditUser", new { Id = model.Id });
        }




        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _authManager.FindUserByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {Id} cannor be found";
                return View("NotFound");
            }
            else
            {

                var result = await _authManager.DeleteUserAsync(user);
                if (result.Succeeded)
                {
                    TempData["AlertMsg"] = "User Deleted!";
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("ListUsers");

        }




        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _authManager.FindRoleByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _authManager.DeleteRoleAsync(role);
                if (result.Succeeded)
                {
                    TempData["AlertMsg"] = "Role Deleted!";
                    return RedirectToAction("ListRoles");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View("ListUsers");
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "Administrator");
        }


    }

}






