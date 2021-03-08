using Microsoft.AspNetCore.Mvc;
using PayFair.WebApi.DAL.Repositories;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayFair.WebApi.Controllers
{
    [Route("api/usersauth")]
    [ApiController]
    public class UsersAuthController : ControllerBase
    {
        public UsersAuthController(IUsersAuthRepository uar)
        {
            var a = uar;
        }

        [HttpPut]
        public async Task<ActionResult> Login([FromBody] UserAuthDTO userAuth)
        {
            // UserAuthDTO userAuth = new UserAuthDTO();
            using (UnitOfWork uow = new UnitOfWork())
            {
                UsersAuthRepository usersAuthRepository = new UsersAuthRepository(uow);
                userAuth = await usersAuthRepository.Login(userAuth);
            }

            if (userAuth == null)
                return Conflict();
            else
            {
                string token = SessionManager.StartNewSession(userAuth.Id);
                AuthTokenDTO authToken = new AuthTokenDTO(token);
                authToken.UserId = userAuth.Id;
                return Ok(authToken);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserAuthDTO userAuth)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    UsersAuthRepository usersAuthRepository = new UsersAuthRepository(uow);
                    bool exist = await usersAuthRepository.Exist(userAuth);

                    if (!exist)
                    {
                        await usersAuthRepository.Add(userAuth);
                        userAuth = await usersAuthRepository.Login(userAuth);

                        UserDTO userDTO = new UserDTO
                        {
                            Id = userAuth.Id,
                            Name = userAuth.Email
                        };

                        UsersRepository usersRepository = new UsersRepository(uow);
                        await usersRepository.Add(userDTO, true);

                        uow.Commit();
                        return Ok();
                    }
                    else
                        return this.Conflict();
                }
                catch (Exception e)
                {
                    uow.Rollback();
                    return this.Conflict(e);
                }
            }
        }
    }
}
