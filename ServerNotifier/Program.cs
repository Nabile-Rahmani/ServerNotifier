using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Steam.Query;

namespace ServerNotifier
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Notifier notifier = new Notifier();
            notifier.PlayerCountReached += (n, d) =>
            {
                Notification notification = Notification.Create();
                notification.Summary = $"{d.Info.Name} has players !";
                notification.Body = $"Players: {d.Info.Players}/{d.Info.MaxPlayers}, Map: {d.Info.Map}";

                notification.Show();
            };

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                switch (arg)
                {
                    case "-c":
                    case "--player-count-trigger":
                        notifier.PlayerCountTrigger = byte.Parse(args[++i]);
                        break;
                    case "-d":
                    case "--update-delay":
                        notifier.UpdateDelay = TimeSpan.FromSeconds(double.Parse(args[++i]));
                        break;
                    case "-h":
                    case "--help":
                        Console.WriteLine($"Usage: {Path.GetFileName(Assembly.GetEntryAssembly().Location)} [options] [address:port...]");
                        Console.WriteLine();
                        Console.WriteLine("Options:");
                        Console.WriteLine("\t-c, --player-count-trigger <byte>: Notify when this amount of players is reached.");
                        Console.WriteLine("\t-d, --update-delay <double>: Frequency of queries (in seconds).");
                        Console.WriteLine("\t-h, --help: Prints this help.");
                        return;
                    default:
                        Uri address;

                        if (!Uri.TryCreate("steam://" + arg, UriKind.Absolute, out address))
                            continue;

                        notifier.Servers.Add(new Server(new IPEndPoint(Dns.GetHostAddresses(address.Host).First(), address.Port)));
                        break;
                }
            }

            notifier.Run();
        }
    }
}
