using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public string username { get; set; }
        public int Port { get; set; }
        public string password { get; set; } //Consider hashing these like with the account controller
    }
}
