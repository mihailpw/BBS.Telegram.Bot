using BBS.Telegram.Bot.Example.Forms.UserInfoForm.Steps;
using BBS.Telegram.Bot.Form;
using BBS.Telegram.Bot.Form.Factories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Example.Forms.UserInfoForm;

public class FormStatesFactory : BotCommandFormStatesFactory
{
    private readonly ITelegramBotClient _botClient;

    public FormStatesFactory(ITelegramBotClient botClient)
    {
        _botClient = botClient;
        RegisterFactory("command1", CreateUserInfoFormState);
    }

    private FormState CreateUserInfoFormState(Message message)
    {
        var rootState = new AskNameStep(
            _botClient,
            new AskGenderStep(
                _botClient,
                new AskOtherGenderStep(_botClient)));
        
        var ctx = new FormContext(message.Chat.Id, new UserInfoBag());
        var formState = new FormState(rootState, ctx, new UserInfoFormCallbacks(_botClient));
        return formState;
    }
}