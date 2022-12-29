using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BBS.Telegram.Bot.Example.Forms;

public class BotBackgroundService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;

    public BotBackgroundService(ITelegramBotClient botClient, IUpdateHandler updateHandler)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _botClient.StartReceiving(_updateHandler, cancellationToken: stoppingToken);
        return Task.CompletedTask;
    }
}