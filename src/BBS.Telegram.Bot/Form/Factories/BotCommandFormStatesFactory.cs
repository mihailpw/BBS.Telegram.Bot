using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form.Factories;

public class BotCommandFormStatesFactory : IFormStatesFactory
{
    private readonly Dictionary<string, Func<Message, FormState>> _factories = new();

    public bool TryCreate(Update update, out FormState formState)
    {
        if (update.Message is { Text: {} } message)
        {
            var command = message.Text;
            if (command.StartsWith('/'))
            {
                command = command.Split('@').First();
                if (_factories.TryGetValue(command, out var factory))
                {
                    formState = factory(message);
                    return true;
                }
            }
        }

        formState = default!;
        return false;
    }

    protected void RegisterFactory(string command, Func<Message, FormState> factory)
    {
        _factories[$"/{command.TrimStart('/')}"] = factory;
    }
}