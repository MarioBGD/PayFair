using Dapper;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories
{
    public class UsersAuthRepository : Repository<UserAuthDTO>, IUsersAuthRepository
    {

        public UsersAuthRepository(IUnitOfWork unitOfWork) : base(unitOfWork.Connection, unitOfWork.Transaction, "UsersAuth")
        {
        }

        public async Task<int> Register(UserAuthDTO userAuthDTO)
        {

            UserAuthDTO result = await Connection.QuerySingleOrDefaultAsync<UserAuthDTO>($"SELECT * FROM {TableName} WHERE Email='{userAuthDTO.Email}'", null, Transaction);
            if (result == null)
            {
                return await Connection.ExecuteAsync($"INSERT INTO {TableName} (Email, Password) VALUES ('{userAuthDTO.Email}', '{userAuthDTO.Password}')", null, Transaction);
            }

            return -1;

        }

        public async Task<UserAuthDTO> Login(UserAuthDTO userAuthDTO)
        {

            return await Connection.QuerySingleOrDefaultAsync<UserAuthDTO>($"SELECT * FROM {TableName} WHERE Email='{userAuthDTO.Email}' AND Password='{userAuthDTO.Password}'", null, Transaction);
        }

        public async Task<bool> Exist(UserAuthDTO userAuthDTO)
        {
            return await Connection.QueryFirstOrDefaultAsync<UserAuthDTO>($"SELECT * FROM {TableName} WHERE Email='{userAuthDTO.Email}'", null, Transaction) != null;
        }
    }
}
