using Microsoft.AspNetCore.Mvc;
using PayFair.WebApi.DAL.Repositories;
using PayFair.DTO.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PayFair.WebApi.SessionManager;

namespace PayFair.WebApi.Controllers
{
    [Route("api/memberships")]
    [ApiController]
    public class MembershipsController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Add([FromHeader] string Authentication, [FromQuery] string userId, [FromQuery] string groupId)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            int userIdInt = 0;
            int groupIdInt = 0;
            if (!int.TryParse(userId, out userIdInt) || !int.TryParse(userId, out groupIdInt))
                return BadRequest("Bad id");

            using (UnitOfWork uow = new UnitOfWork())
            {
                MembershipRepository membershipRepo = new MembershipRepository(uow);
                MembershipDTO membership = await membershipRepo.GetByUserAndGroupId(userIdInt, groupIdInt);

                if (membership != null)
                {
                    if (membership.Status == 0 && userIdInt == sessionInfo.UserId)
                    {
                        membership.Status = 1;
                        await membershipRepo.Update(membership);
                    }
                    else
                    {
                        return Ok();
                    }
                }
                else
                {
                    membership = new MembershipDTO
                    {
                        UserId = userIdInt,
                        GroupId = groupIdInt,
                        Status = 0
                    };
                    membership.Id = await membershipRepo.Add(membership);
                }

                uow.Commit();
                return Ok(membership);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetMemberships([FromHeader] string Authentication, [FromQuery] string groupId)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();


            using (UnitOfWork uow = new UnitOfWork())
            {
                MembershipRepository membershipsRepo = new MembershipRepository(uow);
                IEnumerable<MembershipDTO> memberships = null;

                int groupIdInt = 0;
                if (string.IsNullOrEmpty(groupId) || !int.TryParse(groupId, out groupIdInt) || groupIdInt <= 0)
                {
                    //get by user
                    memberships = await membershipsRepo.GetAllByUserId(sessionInfo.UserId);
                }
                else
                {
                    //get by group
                    memberships = await membershipsRepo.GetAllByGroupId(groupIdInt);
                }

                return Ok(memberships);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromHeader] string Authentication, [FromQuery] string userId, [FromQuery] string groupId)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();


            int userIdInt = 0;
            int groupIdInt = 0;
            if (!int.TryParse(userId, out userIdInt) || !int.TryParse(userId, out groupIdInt))
                return BadRequest("Bad id");

            using (UnitOfWork uow = new UnitOfWork())
            {
                MembershipRepository membershipsRepo = new MembershipRepository(uow);
                MembershipDTO membership = await membershipsRepo.GetByUserAndGroupId(userIdInt, groupIdInt);

                if (membership.Status == 3)
                    return BadRequest("cannot delete the owner");

                await membershipsRepo.Remove(membership.Id);
                uow.Commit();
                return Ok();
            }
        }
    }
}
