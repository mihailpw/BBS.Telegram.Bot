using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form.Steps;

public abstract class AutoDeletableFormStepBase : IFormStep
{
    protected AutoDeletableFormStepBase(ITelegramBotClient botClient)
    {
        BotClient = botClient;
    }
    
    protected ITelegramBotClient BotClient { get; }

    public async Task RenderQuestionAsync(IFormContext ctx, Update request, CancellationToken token)
    {
        CaptureMessage(ctx, request);
        await RenderQuestionInternalAsync(ctx, request, token);
    }

    public async Task<IFormStep?> ExecuteAsync(IFormContext ctx, Update request, CancellationToken token)
    {
        CaptureMessage(ctx, request);
        
        var nextStep = await ExecuteInternalAsync(ctx, request, token);
        if (nextStep != this)
        {
            await DeleteCapturedMessagesAsync(ctx, token);
        }

        return nextStep;
    }

    protected abstract Task RenderQuestionInternalAsync(IFormContext ctx, Update request, CancellationToken token);
    protected abstract Task<IFormStep?> ExecuteInternalAsync(IFormContext ctx, Update request, CancellationToken token);

    protected void CaptureMessage(IFormContext ctx, Update update)
    {
        if (update.Message is { } message)
            CaptureMessage(ctx, message);
    }
    
    protected void CaptureMessage(IFormContext ctx, Message message)
    {
        var messagesIds = GetMessagesIds(ctx);
        messagesIds.Add(message.MessageId);
    }

    protected async Task DeleteCapturedMessagesAsync(IFormContext ctx, CancellationToken token)
    {
        var messagesIds = GetMessagesIds(ctx);
        var cachedMessageIds = messagesIds.ToList();
        messagesIds.Clear();

        await Task.WhenAll(cachedMessageIds.Select(async m =>
        {
            try
            {
                await BotClient.DeleteMessageAsync(ctx.ChatId, m, token);
            }
            catch (ApiRequestException e) when (e.ErrorCode == 400)
            {
                // ignore
            }
        }));
    }

    private HashSet<int> GetMessagesIds(IFormContext ctx)
    {
        return ctx.GetValue(
            $"{nameof(AutoDeletableFormStepBase)}_CapturedMessageIds",
            () => new HashSet<int>())!;
    }
}