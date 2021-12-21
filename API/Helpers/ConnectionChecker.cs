using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class ConnectionChecker
    {
        Ping ping = new Ping();
        PingReply result; //I already have code that uses something like this. Its in RodizioSmartRestuarant.Helpers.LANController, check through it to see
        public bool CheckConnection()
        {
            /*result = ping.Send(IPAddress.Parse("8.8.8.8"));

            if (result.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }*/

            return true;
        }
    }
}
