using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using PicPaySimplificado.Application;
using PicPaySimplificado.Domain;
using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService  _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    [Route("createUser")]
    public async Task<IActionResult> CreateSeller([FromBody] CreateUsersRequest user)
    {
        try
        {
            await _usersService.CreateUserAsync(user.FullName, user.Document, user.Email,
                user.Password);
            
            return Ok(new {Message = "User created successfully"});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet]
    [Route("getUserByDocument")]
    public async Task<IActionResult> GetUserByDocument([FromQuery] Document document)
    {
        try
        {
            var user = await _usersService.FindUserByDocumentAsync(document.DocumentNumber);
            return Ok(new
            {
                Document = user.GetDocument(),
                Email = user.GetEmail(),
                Balance = user.GetBalance(),
                UserType = user.GetUserType().GetDisplayName()
            });
        }
        catch
        {
            return BadRequest(new { Message = "User not found" });
        }
    }

    [HttpGet("getUserById")]
    public async Task<IActionResult> GetUserById([FromQuery] string guid)
    {
        try
        {
            // Tenta buscar o usuário pelo GUID
            var user =  _usersService.FindUserByIdAsync(guid);

            if (user == null)
                return NotFound(new { Message = "User not found" });

            return Ok(new
            {
                Document = user.GetDocument(),
                Email = user.GetEmail(),
                Balance = user.GetBalance(),
                UserType = user.GetUserType().GetDisplayName()
            });
        }
        catch (Exception ex)
        {

            // Retornar uma resposta de erro interna mais informativa
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocorreu um erro inesperado", Exception = ex.Message });
        }
    }

}

public class CreateUsersRequest
{
    public string FullName { get; set; }
    public Document Document { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}