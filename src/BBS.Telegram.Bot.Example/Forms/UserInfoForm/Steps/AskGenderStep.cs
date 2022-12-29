using BBS.Telegram.Bot.Form;
using BBS.Telegram.Bot.Form.Steps;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BBS.Telegram.Bot.Example.Forms.UserInfoForm.Steps;

public class AskGenderStep : AutoDeletableFormStepBase
{
    private readonly IFormStep _otherGenderNext;

    private const string Male = "Male";
    private const string Female = "Female";
    private const string Other = "Other";

    public AskGenderStep(ITelegramBotClient botClient, IFormStep otherGenderNext)
        : base(botClient)
    {
        _otherGenderNext = otherGenderNext;
    }

    protected override async Task RenderQuestionInternalAsync(
        IFormContext ctx, Update request, CancellationToken token)
    {
        var rkm = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                new(Male),
                new(Female)
            },
            new []{new KeyboardButton(Other)}
        });

        var response = await BotClient.SendTextMessageAsync(
            ctx.ChatId,
            "What is your gender?",
            replyMarkup: rkm,
            cancellationToken: token);
        CaptureMessage(ctx, response);
    }

    protected override async Task<IFormStep?> ExecuteInternalAsync(
        IFormContext ctx, Update request, CancellationToken token)
    {
        if (request.Message is not { } message)
            return await RenderErrorAsync(ctx, token);

        var bag = ctx.GetBag<UserInfoBag>();
        
        switch (message.Text)
        {
            case Male:
                bag.GenderMale = true;
                break;
            case Female:
                bag.GenderMale = false;
                break;
            case Other:
                return _otherGenderNext;
            default:
                return await RenderErrorAsync(ctx, token);
        }

        return null;
    }

    private async Task<IFormStep> RenderErrorAsync(IFormContext ctx, CancellationToken token)
    {
        var response = await BotClient.SendTextMessageAsync(
            ctx.ChatId,
            "Wrong gender. Try again",
            cancellationToken: token);
        CaptureMessage(ctx, response);
        return this;
    }
}