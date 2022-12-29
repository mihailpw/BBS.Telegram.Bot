using BBS.Telegram.Bot.Form;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Example.Forms.UserInfoForm;

public class UserInfoFormCallbacks : IFormCallbacks
{
    private readonly ITelegramBotClient _botClient;

    public UserInfoFormCallbacks(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public Task OnStartAsync(FormContext formContext, Update request, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    public async Task OnCompletedAsync(FormContext formContext, Update request, CancellationToken token)
    {
        var bag = formContext.GetBag<UserInfoBag>();
        await _botClient.SendTextMessageAsync(formContext.ChatId,
            $"User '{bag.Name}', GenderMale:{bag.GenderMale}, GenderOther:{bag.GenderOther}",
            cancellationToken: token);
    }
}