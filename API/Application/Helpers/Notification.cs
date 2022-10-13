using RodizioSmartKernel.Application.Interfaces;

namespace API.Application.Helpers
{
    public class Notification : INotification
    {
        public void CreateNotification(string message)
        {
            System.Console.WriteLine("Well you don't put logic to have the unique solution for the notification but here is your message\n" +
                $"{message}");
        }
    }
}
