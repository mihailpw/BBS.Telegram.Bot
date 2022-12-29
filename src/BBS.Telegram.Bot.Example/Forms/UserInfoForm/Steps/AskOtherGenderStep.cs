using BBS.Telegram.Bot.Form;
using BBS.Telegram.Bot.Form.Steps;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BBS.Telegram.Bot.Example.Forms.UserInfoForm.Steps;

public class AskOtherGenderStep : AutoDeletableFormStepBase
{
    public AskOtherGenderStep(ITelegramBotClient botClient)
        : base(botClient)
    {
    }
    
    protected override async Task RenderQuestionInternalAsync(
        IFormContext ctx, Update request, CancellationToken token)
    {
        var rkm = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Smth else")
            },
        });
        var response = await BotClient.SendTextMessageAsync(
            ctx.ChatId,
            "What is other gender?",
            replyMarkup: rkm,
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
            return await RenderErrorAsync(ctx, "Empty gender", token);

        if (text.Contains('/'))
            return await RenderErrorAsync(ctx, "Wrong gender", token);

        var bag = ctx.GetBag<UserInfoBag>();
        
        bag.GenderOther = text;

        return null;
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