using TMPro;
using UnityEngine;

public class ToggleGravityFieldCommand : TerminalCommand
{
    public GravityZone zone;

    protected override string GetDisplayText()
    {
        return zone.isActive 
            ? "<color=#0FF>On</color>" 
            : "<color=#F80>Off</color>";
    }

    public override void Execute()
    {
        if (!canBeInteracted) return;
        zone.ToggleActive();
    }
}