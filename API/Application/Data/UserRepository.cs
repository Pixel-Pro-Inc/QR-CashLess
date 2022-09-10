using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Data
{
    [Obsolete]
    // OBSOLETE: We used this when we were working with EF Core and stuff. But now we on Firebase so itsn't not necessary
    public class UserRepository
    {

        /*
          public Task<AppUser> GetUserByIdAsync (int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Username == username);
        }
         */

    }
}
