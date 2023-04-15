using Microsoft.AspNetCore.Identity;
using NCB.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Services.Interfaces
{
    public interface IAuthManager
    {


        Task<StatusDTO> LoginAsync(LoginDTO model);

        Task LogoutAsync();

        Task<StatusDTO> RegisterAsync(UserDTO model);

        Task<IdentityResult> CreateUserAsync(UserDTO user);

        Task<IdentityResult> UpdateUserAsync(UserDTO user);

        Task<IdentityResult> DeleteUserAsync(UserDTO user);

        Task<IdentityRole> FindRoleByIdAsync(string id);

        Task<UserDTO> FindUserByIdAsync(string id);

        Task<bool> IsInRoleAsync(UserDTO model, string role);

        Task<List<UserDTO>> GetAllUsersAsync();

        List<RoleDTO> GetAllRolesAsync();

        //List<List<TransactionDTO>> GetAllTransactionsAsync();

        Task<bool> RoleExistsAsync(string name);

        Task<IdentityResult> AddUserToRoleAsync(UserDTO model, string role);

        Task<IdentityResult> AddUserToRolesAsync(UserDTO model, IList<string> roles);

        Task<IdentityResult> RemoveUserFromRoleAsync(UserDTO model, string role);
        Task<IdentityResult> RemoveUserFromRolesAsync(UserDTO model, IList<string> roles);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
        Task<IList<string>> GetUserRolesAsync(UserDTO model);
        
    }
}
