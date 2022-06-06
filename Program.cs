using System;

namespace GetRequests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RequestsManager manager = new RequestsManager("c8ecff17-bd25-458c-8f2c-15c40927b34f");
            Console.WriteLine(manager.GetEventName());
            var team = manager.GetEventData();
            var sid = RequestsManager.GetSID("vasya", "1337");
            Console.ReadLine();
        }
    }
}
