using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Users.Commands.DeleteUser;
using Ticketing.Application.Features.Users.Commands.UpdateUser;
using Ticketing.Application.Features.Users.Dtos;
using Ticketing.Application.Features.Users.Queries.GetAllUsersQuery;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ApiController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
                UserDto createdUser = await _mediator.Send(command);
                return Ok(new{Message = "Kullanıcı başarıyla oluşturuldu",User = createdUser});
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
                UserDto updatedUser = await _mediator.Send(command);
                return Ok(new{Message = "Kullanıcı başarıyla güncellendi",User = updatedUser});
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
                var command = new DeleteUserCommand(id);
                await _mediator.Send(command);
                return Ok(new{Message = "Kullanıcı başarıyla silindi"});
        }
    }
}
