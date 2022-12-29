using BBS.Telegram.Bot.Form;
using BBS.Telegram.Bot.Form.Steps;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Example.Forms.UserInfoForm.Steps;

public class AskNameStep : AutoDeletableFormStepBase
{
    private readonly IFormStep _nextStep;

    public AskNameStep(ITelegramBotClient botClient, IFormStep nextStep)
        : base(botClient)
    {
        _nextStep = nextStep;
    }

    protected override async Task RenderQuestionInternalAsync(
        IFormContext ctx, Update request, CancellationToken token)
    {
        var response = await BotClient.SendTextMessageAsync(
            ctx.ChatId,
            "What is your name?",
            cancellationToken: token);
        CaptureMessage(ctx, response);
    }

    protected override async Task<IFormStep?> ExecuteInternalAsync(
        IFormContext ctx, Update request, CancellationToken token)
    {
        if (request.Message is not { } message)
            return await RenderErrorAsync(ctx, "Unexpected response type", token);

        var text = message.Text;
        if (string.IsNullOrWhiteSpace(text))
            return await RenderErrorAsync(ctx, "Empty name", token);

        if (text.Contains('/'))
            return await RenderErrorAsync(ctx, "Wrong name", token);

        var bag = ctx.GetBag<UserInfoBag>();
        
        bag.Name = text;

        return _nextStep;
    }

    private async Task<IFormStep> RenderErrorAsync(
        IFormContext ctx, string message, CancellationToken token)
    {
        var response = await BotClient.SendTextMessageAsync(
            ctx.ChatId,
            $"{message}. Try again",
            cancellationToken: token);
        CaptureMessage(ctx, response);
        return this;
    }
}