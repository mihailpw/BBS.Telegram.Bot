namespace BBS.Telegram.Bot.Form.Infra;

public interface IFormStatesRepository
{
    void AddForm(long chatId, long userId, FormState formState);
    bool TryResolveActiveForm(long chatId, long userId, out FormState? formState);
}