using TMPro;
using UnityEngine;

public class TogglePortalCommand : TerminalCommand
{
    public Portal portal;

    protected override string GetDisplayText()
    {
        return portal.isActive 
            ? "<color=#0FF>On</color>" 
            : "<color=#F80>Off</color>";
    }

    public override void Execute()
    {
        if (!canBeInteracted) return;
        portal.ToggleActive();
    }
}