using API.Entities;
using RodizioSmartKernel.Entities;
using RodizioSmartKernel.Interfaces.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITokenService: IBaseService
    {
        string CreateToken(AppUser user);
    }
}
