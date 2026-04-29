using TMPro;
using UnityEngine;

public class ToggleGravityFieldCommand : TerminalCommand
{
    public GravityZone zone;
    bool active;

    void Start()
    {
        active = zone.isActive;
    }

    void Update()
    {
        base.Update();
        if(!canBeInteracted) return;
        display.text = active 
            ? "<color=#0FF>On</color>" 
            : "<color=#F80>Off</color>";
    }


    public override void Execute()
    {
        if (!canBeInteracted) return;
        
        base.Execute();
        zone.ToggleActive();
        active = !active;
    }
}
