using UnityEngine;

public class TimedGravityChangerCommand : TimedCommand
{
    [Header ("Target")]
    public GravityDirection targetGravity;
    public GravityDirection baseGravity;

    protected override void OnStart() => GravityManager.ChangeWorldGravity(targetGravity.GetVector());
    protected override void OnEnd() => GravityManager.ChangeWorldGravity(baseGravity.GetVector());

    protected override string GetTrivialDisplay2Text() => "change_to_"+targetGravity.GetGravityText();
}
