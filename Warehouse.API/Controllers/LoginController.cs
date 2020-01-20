using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Warehouse.API.Security;
using Warehouse.Infrastructure.Data;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly WarehouseDBContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(WarehouseDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Auth")]
        public ActionResult Authorize()
        {
            var userName = Request.Form["userName"].ToString();
            var password = Request.Form["password"].ToString();

            var tokenProvider = new JWTProvider(_context, _configuration);
            var token = tokenProvider.GetToken(userName, password);

            if (token == null)
            {
                return Ok(new
                {
                    userName,
                    token=string.Empty
                });
            }

            return Ok(new {
                userName,
                token
            });
        }
    }
}