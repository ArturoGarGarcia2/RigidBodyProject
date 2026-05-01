using UnityEngine;

public abstract class OneShotCommand : TerminalCommand
{
    [Header ("State")]
    private bool used = false;

    public override void Execute()
    {
        if (used) return;

        used = true;
        OnExecute();

        SetInteractable(false);
    }

    protected abstract void OnExecute();

    protected override string GetDisplayText() =>
        used
        ? "<color=#555>Used</color>"
        : "<color=#0F0>Execute</color>";
}