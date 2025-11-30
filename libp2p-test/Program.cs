using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nethermind.Libp2p;

namespace libp2p_test;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.Write("Nickname: ");
        var nickName = Console.ReadLine();

        Regex omittedLogs = new(".*(MDnsDiscoveryProtocol|IpTcpProtocol).*");

        var services = new ServiceCollection()
            .AddLibp2p(builder => builder.WithPubsub())
            .AddLogging(builder =>
            {
                builder.SetMinimumLevel(args.Contains("--trace") ? LogLevel.Trace : LogLevel.Information)
                    .AddFilter((_, type, lvl) => !omittedLogs.IsMatch(type!));


                    builder.AddSimpleConsole(l =>
                    {
                        l.SingleLine = true;
                        l.TimestampFormat = "[HH:mm:ss.fff]";
                    });
            })
            .BuildServiceProvider();

        var chatService = new ChatService(services);
        await chatService.StartAsync();

        while (true)
        {
            var msg = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(msg))
                continue;

            if (msg == "exit")
                break;

            chatService.Publish(msg, nickName);
        }

        chatService.Stop();
    }
}