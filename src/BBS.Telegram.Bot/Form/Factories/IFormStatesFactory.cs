using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form.Factories;

public interface IFormStatesFactory
{
    bool TryCreate(Update update, out FormState formState);
}