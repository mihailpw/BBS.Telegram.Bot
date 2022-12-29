using BBS.Telegram.Bot.Form.Factories;
using BBS.Telegram.Bot.Utils;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form.Infra;

public class FormHandlerDecorator : IUpdateHandler
{
    private readonly IUpdateHandler? _next;
    private readonly IFormStatesFactory _formStatesFactory;
    private readonly IFormStatesRepository _formStatesRepository;

    public FormHandlerDecorator(
        IUpdateHandler? next,
        IFormStatesFactory formStatesFactory,
        IFormStatesRepository formStatesRepository)
    {
        _next = next;
        _formStatesFactory = formStatesFactory;
        _formStatesRepository = formStatesRepository;
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.GetChatId();
        if (chatId.HasValue)
        {
            var userId = update.GetUserId();
            if (userId.HasValue)
            {
                if (!_formStatesRepository.TryResolveActiveForm(chatId.Value, userId.Value, out var formState))
                {
                    if (_formStatesFactory.TryCreate(update, out formState))
                    {
                        _formStatesRepository.AddForm(chatId.Value, userId.Value, formState);
                    }
                }

                if (formState != null)
                {
                    await formState.ProcessMessageAsync(update, cancellationToken);
                    return;
                }
            }
        }

        if (_next != null)
                await _next.HandleUpdateAsync(botClient, update, cancellationToken);
    }

    public async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (_next != null)
            await _next.HandlePollingErrorAsync(botClient, exception, cancellationToken);
    }
}