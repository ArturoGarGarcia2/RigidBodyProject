using UnityEngine;

public abstract class ToggleCommand : TerminalCommand
{
    [Header ("State")]
    protected bool isActive = true;

    public override void Execute()
    {
        isActive = !isActive;
        OnToggle();
    }

    protected abstract void OnToggle();

    protected override string GetDisplayText() =>
        isActive
        ? "<color=#F80>Off</color>"
        : "<color=#0FF>On</color>";

        
    protected override string GetTrivialDisplay1Text() => "toggle_terminal";
}
