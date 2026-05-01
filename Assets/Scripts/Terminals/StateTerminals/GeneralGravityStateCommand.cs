using UnityEngine;

public class GeneralGravityStateCommand : StateCommand
{
    public GravityDirection direction;

    protected override void OnExecute(){
        GravityManager.ChangeWorldGravity(direction.GetVector());
    }

    protected override string GetDisplayText() =>
        sameState
        ? "<size=75%><color=#555>Executed</color></size>"
        : "<size=75%><color=#FFF>Execute</color></size>";
    
    void Update(){
        base.Update();
        sameState = GravityManager.worldGravityDir == direction.GetVector();
    }
}
