using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.WebAPI.Abstractions;

namespace Ticketing.WebAPI.Controllers
{
    public class AdminController : ApiController
    {

        public AdminController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Secure()
        {
            return Ok(new { message = "Bu veri sadece admin için erişilebilir!" });
        }

        [HttpGet]
        public IActionResult Public()
        {
            return Ok(new { message = "Bu veri tüm giriş yapmış kullanıcılar için." });
        }
    }
}
