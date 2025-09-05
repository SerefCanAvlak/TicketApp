using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.Features.Auth.Login;
using Ticketing.Application.Features.Auth.Register;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
        {
                var response = await _mediator.Send(request, cancellationToken);
                return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(new { UserId = userId, Message = "User registered successfully" });
        }
    }
}