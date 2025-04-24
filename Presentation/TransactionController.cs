using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Application;
using PicPaySimplificado.DTOs;

namespace PicPaySimplificado.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("createTransaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transaction)
    {
        try
        {
            await _transactionService.CreateTransactionAsync(transaction);
            return Ok(new { Message = "Transaction created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message});
        }
    }
}