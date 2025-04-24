using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Application;
using PicPaySimplificado.DTOs;

namespace PicPaySimplificado.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly Transaction _transaction;

    public TransactionController(Transaction transaction)
    {
        _transaction = transaction;
    }

    [HttpPost("createTransaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transaction)
    {
        try
        {
            await _transaction.CreateTransactionAsync(transaction);
            return Ok(new { Message = "Transaction created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message});
        }
    }
}