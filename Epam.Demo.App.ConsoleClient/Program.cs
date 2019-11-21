using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace Epam.Demo.App.ConsoleClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Please type your full name:");
            var fullName = Console.ReadLine();
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new User.UserClient(channel);
            var reply = await client.SaveInfoAsync(new UserRequest() { Fullname = fullName });
            Console.WriteLine(reply.Status);
            Console.ReadKey();
        }
    }
}