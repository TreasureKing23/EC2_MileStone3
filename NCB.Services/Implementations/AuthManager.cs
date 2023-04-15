using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NCB.ModelDTO;
using NCB.Models;
using NCB.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Services.Implementations
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;


        public AuthManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }



        public async Task<StatusDTO> LoginAsync(LoginDTO model)
        {
            var status = new StatusDTO();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                status.StatusCode = 0;

                status.StatusMessage = "Invalid Username";
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.StatusMessage = "Invalid Passowrd";
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = -1;
                status.StatusMessage = "Invalid Passowrd";
                return status;
            }


            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim (ClaimTypes.Role, role));
                }
                status.StatusCode = 1;
                status.StatusMessage = "Logged In Successfully";
            }
            else if (result.IsLockedOut)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User is Locked Out";
            }
            else
            {
                status.StatusCode = 0;
                status.StatusMessage = "Error With Log In";
            }

            return status;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<StatusDTO> RegisterAsync(UserDTO model)
        {
            var status = new StatusDTO();
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                status.StatusCode = 0; 
                status.StatusMessage = "User Already Exists";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Name = model.Name,
                Address = model.Address,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User Creation Failed";
                return status;
            }
            status.StatusCode = 1;
            status.StatusMessage = "You Have Registered Successfully";
            return status;

        }



        public async Task<IdentityResult> CreateUserAsync(UserDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<IdentityResult> UpdateUserAsync(UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Name = model.Name;
            user.Address = model.Address;
            user.Email = model.Email;
            user.UserName = model.UserName;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.DeleteAsync(user);
        }


        public async Task<UserDTO> FindUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            return _mapper.Map<List<UserDTO>>(users);
        }

        //public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        //{
        //    var trans = _userManager.Users.ToList();
        //    return _mapper.Map<List<TransactionDTO>>(trans);
        //}

        public async Task<IdentityResult> AddUserToRoleAsync(UserDTO user, string role)
        {
            return await _userManager.AddToRoleAsync(_mapper.Map<ApplicationUser>(user), role);
        }
        public async Task<IdentityResult> AddUserToRolesAsync(UserDTO model, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.AddToRolesAsync(user, roles);
        }
        public async Task<IdentityResult> RemoveUserFromRoleAsync(UserDTO user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(_mapper.Map<ApplicationUser>(user), role);
        }
        public async Task<IdentityResult> RemoveUserFromRolesAsync(UserDTO model, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<bool> IsInRoleAsync(UserDTO model, string role)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }
        public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }



        public async Task<IdentityResult> DeleteRoleAsync(IdentityRole model)
        {
            var role = _roleManager.FindByIdAsync(model.Id);
            return await _roleManager.DeleteAsync(role.Result);
        }
        public List<RoleDTO> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<List<RoleDTO>>(roles);
        }
        public async Task<IList<string>> GetUserRolesAsync(UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<IdentityRole> FindRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserDTO model)
        {
            return await _userManager.GetClaimsAsync(_mapper.Map<ApplicationUser>(model));
        }


    }
}
 
