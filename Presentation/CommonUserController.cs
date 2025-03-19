using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Application;

namespace PicPaySimplificado.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommonUserController : ControllerBase
    {
        private readonly CommomUserService _service;

        public CommonUserController(CommomUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                await _service.CreateUserAsync(request.FullName, request.Cpf, request.Email, request.Password);

                return Ok(new { Message = "Usu�rio criado com sucesso!" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
