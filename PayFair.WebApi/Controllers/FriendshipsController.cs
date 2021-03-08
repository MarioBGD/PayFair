using Microsoft.AspNetCore.Mvc;
using PayFair.WebApi.DAL.Repositories;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PayFair.WebApi.SessionManager;

namespace PayFair.WebApi.Controllers
{
    [Route("api/friendships")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> Add([FromHeader] string Authentication, [FromQuery] string userId, [FromQuery] string username)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            using (UnitOfWork uow = new UnitOfWork())
            {
                int secondUserId = 0;
                if (!string.IsNullOrEmpty(userId))
                    int.TryParse(userId, out secondUserId);

                if (secondUserId <= 0)
                {
                    if (string.IsNullOrEmpty(username))
                        return NotFound();

                    UsersRepository usersRepository = new UsersRepository(uow);
                    UserDTO user = await usersRepository.GetByName(username);

                    if (user == null)
                        return NotFound("User {username} not found");

                    secondUserId = user.Id;
                }

                if (secondUserId == sessionInfo.UserId)
                    return Conflict("Cannot invite yourself");

                FriendshipsRepository friendshipsRepository = new FriendshipsRepository(uow);
                FriendshipDTO friendship = await friendshipsRepository.GetFriendship(secondUserId, sessionInfo.UserId);

                if (friendship == null)
                {
                    friendship = new FriendshipDTO
                    {
                        UserOneId = sessionInfo.UserId,
                        UserTwoId = secondUserId,
                        Status = 0
                    };
                    await friendshipsRepository.Add(friendship);
                }
                else
                {
                    if (friendship.Status == 0 && friendship.UserOneId == secondUserId)
                    {
                        friendship.Status = 1;
                        await friendshipsRepository.Update(friendship);
                    }
                }

                uow.Commit();
                return Ok(friendship);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromHeader] string Authentication, string userId)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            int userIdNum = 0;
            if (!int.TryParse(userId, out userIdNum))
                return NotFound(userId);

            using (UnitOfWork uow = new UnitOfWork())
            {
                FriendshipsRepository friendshipsRepository = new FriendshipsRepository(uow);
                FriendshipDTO friendship = await friendshipsRepository.GetFriendship(userIdNum, sessionInfo.UserId);

                if (friendship != null)
                {
                    await friendshipsRepository.Remove(friendship.Id);
                }

                uow.Commit();
            }

            return Ok();
        }



        /// <param name="status">-1 - all, 0 requests, 1 friends</param>
        /// <returns>List of users Ids</returns>
        [HttpGet]
        public async Task<ActionResult> GetListOfIds([FromHeader] string Authentication, [FromQuery] string status)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            int statusNum = -1;
            if (!string.IsNullOrEmpty(status))
                int.TryParse(status, out statusNum);

            using (UnitOfWork uow = new UnitOfWork())
            {
                FriendshipsRepository friendshipsRepository = new FriendshipsRepository(uow);
                IEnumerable<FriendshipDTO> friendships = await friendshipsRepository.GetFriendships(sessionInfo.UserId, statusNum);
                return Ok(friendships);
            }
        }
    }
}
