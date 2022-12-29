namespace BBS.Telegram.Bot.Form;

public static class FormContextExt
{
    public static TBag GetBag<TBag>(this IFormContext target) where TBag : IFormBag
    {
        return (TBag)target.Bag;
    }

    public static TValue GetValue<TValue>(this IFormContext target, string key, Func<TValue> defaultValueResolver)
    {
        if (!target.TryGetValue<TValue>(key, out var value))
        {
            value = defaultValueResolver()!;
            target.SetValue(key, value);
        }

        return value;
    }
}