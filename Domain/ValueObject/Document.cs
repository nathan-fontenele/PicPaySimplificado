using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using PicPaySimplificado.Domain;

namespace PicPaySimplificado.ValueObject;

public class Document
{
    public string DocumentNumber { get; set; }
    
    [JsonIgnore]
    public UserType UserType { get; private set; }
    
    public Document() { }
    
    public Document(string documentNumber)
    {
        if (string.IsNullOrEmpty(documentNumber))
        {
            throw new ArgumentException("Document Number cannot be null or empty.");
        }

        if (IsValidCpf(documentNumber))
        {
            DocumentNumber = documentNumber;
            UserType = UserType.Common;
        }
        else if (IsValidCnpj(documentNumber))
        {
            DocumentNumber = documentNumber;
            UserType = UserType.Merchant;
        }
        else
        {
            throw new ArgumentException("Document Number is invalid.");
        }
    }
    
    public bool IsValid(string documentNumber)
    {
        if (documentNumber.Length == 11)
            return IsValidCpf(documentNumber);
        else if (documentNumber.Length == 14)
            return IsValidCnpj(documentNumber);
        else
            return false;
    }

    private bool IsValidCpf(string cpf)
    {
        if (cpf.All(c => c == cpf[0])) return false;

        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult1[i];

        int remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        tempCpf += digit1;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult2[i];

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf.EndsWith($"{digit1}{digit2}");
    }

    private bool IsValidCnpj(string cnpj)
    {
        if (cnpj.All(c => c == cnpj[0])) return false;

        int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCnpj = cnpj.Substring(0, 12);
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * mult1[i];

        int remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        tempCnpj += digit1;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * mult2[i];

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cnpj.EndsWith($"{digit1}{digit2}");
    }
    
    public override string ToString()
    {
        return DocumentNumber;
    }

}