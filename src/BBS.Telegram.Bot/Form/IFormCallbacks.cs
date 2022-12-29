using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form;

public interface IFormCallbacks
{
    Task OnStartAsync(FormContext formContext, Update request, CancellationToken token);
    Task OnCompletedAsync(FormContext formContext, Update request, CancellationToken token);
}