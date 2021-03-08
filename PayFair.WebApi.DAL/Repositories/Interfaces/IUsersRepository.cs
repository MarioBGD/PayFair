using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserDTO> GetWithoutImage(int id);
        Task<UserDTO> GetByName(string name);
    }
}
