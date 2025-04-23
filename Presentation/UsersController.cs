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

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsersRequest user)
    {
        try
        {
            await _usersService.CreateUserAsync(user.FullName, user.Document, user.Email, user.Password);
            
            return Ok(new { Message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _usersService.GetUsers();
            return Ok(users.ToList());
        }
        catch (Exception e)
        {
            return BadRequest(new { Message = e.Message });
        }
    }

    [HttpGet("getUserByDocument")]
    public async Task<IActionResult> GetUserByDocument([FromQuery(Name = "DocumentNumber")] string documentNumber)
    {
        try
        {
            var document = new Document(documentNumber);
            var user = await _usersService.FindUserByDocumentAsync(document.DocumentNumber);

            return Ok(new
            {
                Document = user.GetDocument(),
                Email    = user.GetEmail(),
                Balance  = user.GetBalance()
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
            var user =  await _usersService.FindUserByIdAsync(guid);

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