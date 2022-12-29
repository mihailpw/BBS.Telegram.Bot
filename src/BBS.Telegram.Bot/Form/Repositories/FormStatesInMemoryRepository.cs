namespace BBS.Telegram.Bot.Form.Infra;

public class FormStatesInMemoryRepository : IFormStatesRepository
{
    private readonly Dictionary<(long, long), Stack<FormState>> _activeForms = new();

    public void AddForm(long chatId, long userId, FormState formState)
    {
        var key = (chatId, userId);
        if (!_activeForms.TryGetValue(key, out var stack))
        {
            stack = new Stack<FormState>();
            _activeForms.Add(key, stack);
        }

        stack.Push(formState);
    }

    public bool TryResolveActiveForm(long chatId, long userId, out FormState? formState)
    {
        var key = (chatId, userId);
        if (_activeForms.TryGetValue(key, out var stack))
        {
            while (stack.TryPeek(out var latestForm))
            {
                if (latestForm.IsCompleted)
                {
                    stack.Pop();
                    if (stack.Count == 0)
                    {
                        _activeForms.Remove(key);
                    }
                }
                else
                {
                    formState = latestForm;
                    return true;
                }
            }
        }

        formState = default!;
        return false;
    }
}