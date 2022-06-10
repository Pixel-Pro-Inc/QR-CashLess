using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    /// <summary>
    /// SMS type that will be stored in the database
    /// <para> The message sent is not important cause if you know the origin you know what the message is</para>
    /// <code>
    /// if(Origin=="Order_Complete")"Rodizio Express\n\nYour order #{orderNumber} is ready! Go to the till to collect. Thank you for your purchase.\n\nPowered by Pixel Pro"
    /// if(Origin=="Order_Cancel")"Rodizio Express\n\nYour order #{orderNumber} has been cancelled.\n\nPowered by Pixel Pro"
    /// if(Origin=="Account_Reset")"Rodizio Express\n\nThis is your password reset token {token}.\n\nPowered by Pixel Pro"
    /// if(Origin=="Error_Log")"Rodizio Express\n\nWe just had a hiccup. The error message is {errorlog}.\n\nPowered by Pixel Pro"
    /// </code>
    /// <para> This is the code I expect to be used</para>
    /// </summary>
    public class SMS: BaseEntity
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public DateTime DateSent { get; set; }
    }
}
