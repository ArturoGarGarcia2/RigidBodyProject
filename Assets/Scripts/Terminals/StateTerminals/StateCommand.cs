using UnityEngine;

public abstract class StateCommand : TerminalCommand
{
    protected bool sameState;

    public override void Execute()
    {
        if (sameState) return;

        sameState = true;
        OnExecute();
    }
    
    protected abstract void OnExecute();

    protected override string GetDisplayText() =>
        sameState
        ? "<size=75%><color=#555>Executed</color></size>"
        : "<size=75%><color=#0F0>Execute</color></size>";

    protected override void EnableLight(){
        light.SetActive(!sameState);
    }
}
