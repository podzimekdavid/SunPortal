using StuderReader;

namespace SunPortal.Client;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var client = new Client(args[0], args.Length >= 4 ? bool.Parse(args[3]) : false)
            {
                ClientId = Guid.Parse(args[1]),
                Url = args.Length > 2 ? args[2] : "http://localhost:5114" // <-- Debug
            };

            client.Start();
            
            while (true)
            {
                client.CheckQueue();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


    }
}