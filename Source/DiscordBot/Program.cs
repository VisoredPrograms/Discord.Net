using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    internal class Program
    {
        private static DiscordSocketClient Client;
        public static async Task Main()
        {
            var Config = new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All
            };
            Client = new DiscordSocketClient(Config);

            var token = "YOUR_BOT_TOKEN";

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            Client.Log += Log;
            Client.Ready += Ready;

            Client.MessageReceived += MessageReceived;

            // Block this task until the program is closed.
            await Task.Delay(Timeout.Infinite);
        }

        private static Task Log(LogMessage Message)
        {
            Console.WriteLine(Message.ToString());
            return Task.CompletedTask;
        }

        private static Task Ready()
        {
            Console.WriteLine($"{Client.CurrentUser} is connected!");
            return Task.CompletedTask;
        }

        private static async Task MessageReceived(SocketMessage Message)
        {
            if (Message.Author.IsBot) return;
            if (Message.Content.Substring(0,1) != "!") return;

            string CommandName = Message.Content.Substring(1).ToLower();

            switch (CommandName)
            {
                case "help":
                    string s = "Currently, there are two commands - 'ping' and 'help'.";
                    await Message.Channel.SendMessageAsync(s);
                    break;
                case "ping":
                    var Latency = Client.Latency;
                    await Message.Channel.SendMessageAsync($"Pong! Latency is {Latency}ms");
                    break;
                default:
                    await Message.Channel.SendMessageAsync("Sorry. I didn't get that.");
                    break;
            }

            
        }
    }
}
