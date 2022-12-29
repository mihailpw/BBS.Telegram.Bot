using BBS.Telegram.Bot.Form.Steps;
using Telegram.Bot.Types;

namespace BBS.Telegram.Bot.Form;

public sealed class FormState
{
    private readonly IFormStep _rootState;
    private readonly FormContext _ctx;
    private readonly IFormCallbacks _formCallbacks;
    private IFormStep? _currentStep;

    public FormState(IFormStep rootState, FormContext ctx, IFormCallbacks formCallbacks)
    {
        _rootState = rootState;
        _ctx = ctx;
        _formCallbacks = formCallbacks;
    }

    public bool IsCompleted { get; private set; }

    public async Task ProcessMessageAsync(Update request, CancellationToken token)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Form is done");
        
        if (_currentStep == null)
        {
            _currentStep = _rootState;
            await _formCallbacks.OnStartAsync(_ctx, request, token);
            await _currentStep.RenderQuestionAsync(_ctx, request, token);
            return;
        }

        _currentStep = await _currentStep!.ExecuteAsync(_ctx, request, token);
        if (_currentStep == null)
        {
            IsCompleted = true;
            await _formCallbacks.OnCompletedAsync(_ctx, request, token);
        }
        else
        {
            await _currentStep.RenderQuestionAsync(_ctx, request, token);
        }
    }
}