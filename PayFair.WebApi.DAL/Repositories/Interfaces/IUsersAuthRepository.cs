using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PayFair.DTO.Users;

namespace PayFair.WebApi.DAL.Repositories.Interfaces
{
    public interface IUsersAuthRepository : IRepository<UserAuthDTO>
    {
        Task<int> Register(UserAuthDTO userAuthDTO);


        /// <returns>userId</returns>
        Task<UserAuthDTO> Login(UserAuthDTO userAuthDTO);
    }
}
