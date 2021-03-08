using Dapper;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using PayFair.DTO.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories
{
    public class GroupsRepository : Repository<GroupDTO>, IGroupsRepository
    {
        public GroupsRepository(IUnitOfWork unitOfWork) : base(unitOfWork.Connection, unitOfWork.Transaction, "Groups")
        {

        }

        public async Task<IEnumerable<GroupDTO>> GetGroups(List<int> groupIds)
        {
            string query = $"SELECT * FROM {TableName}" +
                $" WHERE "
                + string.Join(" OR ", groupIds.Select(p => $"Id = '{p}'"));

            return await Connection.QueryAsync<GroupDTO>(query, null, Transaction);
        }
    }
}
