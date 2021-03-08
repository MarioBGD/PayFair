using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayFair.WebApi.DAL.Repositories.Interfaces
{
    public interface IFriendshipsRepository : IRepository<FriendshipDTO>
    {
        Task<FriendshipDTO> GetFriendship(int userOneId, int userTwoId);
        Task<IEnumerable<FriendshipDTO>> GetFriendships(int userId, int status = -1);
    }
}
