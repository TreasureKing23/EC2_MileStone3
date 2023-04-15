using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NCB.ModelDTO;
using NCB.Models;

namespace NCB.Web.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {

            CreateMap<Currency, CurrencyDTO>().ReverseMap(); 
            CreateMap<Currency, CreateCurrencyDTO>().ReverseMap(); 
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, CreateAccountDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap(); 
            CreateMap<ApplicationUser, LoginDTO>().ReverseMap(); 
            CreateMap<IdentityRole, RoleDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
        }

    }
}
