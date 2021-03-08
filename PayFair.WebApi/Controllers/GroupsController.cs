using Microsoft.AspNetCore.Mvc;
using PayFairCommon;
using PayFair.WebApi.DAL.Repositories;
using PayFair.DTO;
using PayFair.DTO.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PayFair.WebApi.SessionManager;

namespace PayFair.WebApi.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> Get([FromHeader] string Authentication, [FromQuery] string groupsIds)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            string[] idsStr = groupsIds.Split('|');
            List<int> ids = new List<int>();
            foreach (string idStr in idsStr)
            {
                int id;
                if (int.TryParse(idStr, out id))
                    ids.Add(id);
            }

            IEnumerable<GroupDTO> groups;

            using (UnitOfWork uow = new UnitOfWork())
            {
                GroupsRepository groupsRepository = new GroupsRepository(uow);
                groups = await groupsRepository.GetGroups(ids);
            }

            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromHeader] string Authentication, [FromBody] GroupDTO group)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            if (string.IsNullOrEmpty(group.Name) || string.IsNullOrEmpty(group.DefaultCurrency) ||
                !Currencies.CurrenciesList.Contains(group.DefaultCurrency))
                return BadRequest();

            group.CreatedDate = DateTime.Now;

            using (UnitOfWork uow = new UnitOfWork())
            {
                GroupsRepository groupsRepository = new GroupsRepository(uow);
                group.Id = await groupsRepository.Add(group);

                MembershipDTO membership = new MembershipDTO
                {
                    UserId = sessionInfo.UserId,
                    GroupId = group.Id,
                    Status = 3
                };
                MembershipRepository membershipRepository = new MembershipRepository(uow);
                await membershipRepository.Add(membership);

                uow.Commit();
            }

            return Ok();
        }

        //TODO dodać weryfikacjer czy osoba usuwająca jest adminem i dodać weryfikacje, czy długi pospłacane
        [HttpDelete]
        public async Task<ActionResult> Delete([FromHeader] string Authentication, [FromQuery] string Id)
        {
            if (SessionManager.GetSessionState(Authentication) != SessionManager.SessionState.Authorized)
                return Unauthorized();
            SessionInfo sessionInfo = SessionManager.GetSessionInfo(Authentication);
            if (sessionInfo == null)
                return Unauthorized();

            int groupId = 0;
            if (!int.TryParse(Id, out groupId) || groupId <= 0)
                return BadRequest();

            using (UnitOfWork uow = new UnitOfWork())
            {
                MembershipRepository membershipRepository = new MembershipRepository(uow);
                IEnumerable<MembershipDTO> memberships = await membershipRepository.GetAllByGroupId(groupId);

                if (memberships == null)
                    return NotFound();

                foreach (var membership in memberships)
                    await membershipRepository.Remove(membership.Id);

                GroupsRepository groupsRepository = new GroupsRepository(uow);
                await groupsRepository.Remove(groupId);

                uow.Commit();
            }

            return Ok();
        }
    }
}
