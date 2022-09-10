using RodizioSmartKernel.Entities;
using RodizioSmartKernel.Interfaces.BaseInterfaces;

namespace API.Application.Interfaces
{
    public interface ITokenService: IBaseService
    {
        string CreateToken(AppUser user);
    }
}
