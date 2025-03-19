using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Repositories;

namespace PicPaySimplificado.Application;

public class SellerUserService
{
    private readonly ISellerUserRepository  _sellerUserRepository;

    public SellerUserService(ISellerUserRepository sellerUserRepository)
    {
        _sellerUserRepository = sellerUserRepository;
    }

    public async Task CreateSellerUserAsync(string fullName, string cnpj, string email, string password)
    {
        if (await _sellerUserRepository.CnpjExisteAsync(cnpj))
            throw new Exception("CNPJ já cadastrado");
        
        if(await _sellerUserRepository.CnpjExisteAsync(cnpj))
            throw new Exception("E-mail já cadastrado");
        
        var seller = new SellerUserEntity(fullName, email, cnpj, password);
        await _sellerUserRepository.AddAsync(seller);
    }
}