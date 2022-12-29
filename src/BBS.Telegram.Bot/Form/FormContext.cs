namespace BBS.Telegram.Bot.Form;

public class FormContext : IFormContext
{
    private readonly Dictionary<string, object> _valuesStore = new();

    public FormContext(long chatId, IFormBag bag)
    {
        ChatId = chatId;
        Bag = bag;
    }

    public long ChatId { get; }
    public IFormBag Bag { get; }

    public void SetValue(string key, object value)
    {
        _valuesStore[key] = value;
    }

    public bool TryGetValue<TValue>(string key, out TValue value)
    {
        if (_valuesStore.TryGetValue(key, out var valueObj)
            && valueObj is TValue valueTyped)
        {
            value = valueTyped;
            return true;
        }
        else
        {
            value = default!;
            return false;
        }
    }
}