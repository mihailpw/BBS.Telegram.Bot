using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form.Steps;

public interface IFormStep
{
    Task RenderQuestionAsync(IFormContext ctx, Update request, CancellationToken token);
    Task<IFormStep?> ExecuteAsync(IFormContext ctx, Update request, CancellationToken token);
}