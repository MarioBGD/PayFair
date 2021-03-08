using Microsoft.AspNetCore.Mvc;
using PayFair.WebApi.DAL.Repositories;
using PayFair.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayFair.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
       

        [HttpGet]
        public async Task<ActionResult> Get(string usersids, bool withImage)
        {
            string[] ids = usersids.Split(';');
            List<UserDTO> users = new List<UserDTO>();

            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    UsersRepository usersRepo = new UsersRepository(uow);

                    foreach (string id in ids)
                    {
                        int userId = 0;
                        if (int.TryParse(id, out userId))
                        {
                            UserDTO user;
                            if (withImage) user = await usersRepo.Get(userId);
                            else user = await usersRepo.GetWithoutImage(userId);

                            users.Add(user);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Conflict(e);
            }

            return Ok(users);
        }

    }
}
