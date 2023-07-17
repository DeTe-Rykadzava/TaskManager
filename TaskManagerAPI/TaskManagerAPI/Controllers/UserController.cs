using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Contexts
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public UserController(TaskManagerContext context)
        {
            _context = context;
        }

        // для упрощения логики, чтобы в 1 методе проверять авторизацию и регистрацию пользователей
        public enum SignAction
        {
            In,
            Up
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser([FromQuery][Required] SignAction action,
            [FromBody]UserModel model)
        {
            if (action == SignAction.Up)
            {
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Login = model.Login,
                    Password = model.Password,
                    Name = model.UserName
                };

                _context.Users.Add(newUser);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }

                return Ok(new UserDto() { UserName = newUser.Name, UserId = newUser.Id });
            }
            else if (action == SignAction.In)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == model.Login &&
                                                                         x.Password == model.Password);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(new UserDto() { UserName = user.Name, UserId = user.Id });
            }
            return BadRequest();
        }
      
    }
}
