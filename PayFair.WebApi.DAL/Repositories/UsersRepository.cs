using Dapper;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories
{
    public class UsersRepository : Repository<UserDTO>, IUsersRepository
    {
        public UsersRepository(IUnitOfWork unitOfWork) : base(unitOfWork.Connection, unitOfWork.Transaction, "Users")
        {
        }

        public async Task<UserDTO> GetByName(string name)
        {
            return await Connection.QuerySingleOrDefaultAsync<UserDTO>($"SELECT * FROM {TableName} WHERE Name='{name}'", null, Transaction);

        }

        public async Task<UserDTO> GetWithoutImage(int id)
        {
            return await Get(id);
        }
    }
}
