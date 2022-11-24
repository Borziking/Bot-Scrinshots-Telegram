using PuppeteerSharp;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace BotScreenshot;

//Sends a screenshot when the command is called

class Start
{
    static ITelegramBotClient bot = new TelegramBotClient("Token");

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message?.Text != null)
            {
                if (message.Text.ToLower() == "/timetable")

                    using (var stream = System.IO.File.OpenRead("table.png"))
                    {
                        Telegram.Bot.Types.InputFiles.InputOnlineFile iof = new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream);
                        iof.FileName = "table.png";
                        var send = await botClient.SendPhotoAsync(message.Chat, iof, "");
                    }

                return;
            }
            
        }
    }

    //Takes a screenshot of the specified site


    async static Task Main(string[] args)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
        var page = await browser.NewPageAsync();
        var outputFile = "table.png";
        await page.SetViewportAsync(new ViewPortOptions
        {
            Width = 1250,
            Height = 960
        });
        await page.GoToAsync("URL");
        await page.ScreenshotAsync(outputFile);

        Console.WriteLine("Launch complete!");

        //Start Bot
        
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, 
        };
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }

    private static Task HandleErrorAsync(ITelegramBotClient message, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
}