using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Application;
using PicPaySimplificado.Domain;

namespace PicPaySimplificado.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SellerUserController : ControllerBase
{
    private readonly SellerUserService  _sellerUserService;

    public SellerUserController(SellerUserService sellerUserService)
    {
        _sellerUserService = sellerUserService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSeller([FromBody] CreateSellerRequest sellerUser)
    {
        try
        {
            await _sellerUserService.CreateSellerUserAsync(sellerUser.FullName, sellerUser.Cnpj, sellerUser.Email,
                sellerUser.Password);
            
            return Ok(new {Message = "Lojista cadastrado com sucesso!"});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}

public class CreateSellerRequest
{
    public string FullName { get; set; }
    public string Cnpj { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}