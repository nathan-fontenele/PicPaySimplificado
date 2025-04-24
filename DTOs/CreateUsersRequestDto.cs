using PicPaySimplificado.ValueObject;

namespace PicPaySimplificado.DTOs;

public record CreateUsersRequestDto(string fullname, Document document, decimal balance, string email, string password)
{
}