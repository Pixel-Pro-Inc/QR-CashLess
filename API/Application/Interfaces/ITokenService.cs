using RodizioSmartKernel.Core.Entities;
using RodizioSmartKernel.Application.Interfaces.BaseInterfaces;

namespace API.Application.Interfaces
{
    public interface ITokenService: IBaseService
    {
        string CreateToken(AppUser user);
    }
}
