using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    [Obsolete]
    // OBSOLETE: We end up using config but we are even considering using environment variables. These are left so that we know where we are coming from and could be a 
    //potenial fallback 
    public class CloudinarySettings
    {
        //We might not need this since we using config to make the statements for us
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}
