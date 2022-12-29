namespace BBS.Telegram.Bot.Form;

public interface IFormContext
{
    long ChatId { get; }
    IFormBag Bag { get; }

    void SetValue(string key, object value);
    bool TryGetValue<TValue>(string key, out TValue value);
}