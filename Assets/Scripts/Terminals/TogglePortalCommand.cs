using TMPro;
using UnityEngine;

public class TogglePortalCommand : TerminalCommand
{
    public Portal portal;
    [SerializeField] TMP_Text display;
    bool active;

    void Start()
    {
        active = portal.isActive;
    }

    void Update()
    {
        display.text = active 
            ? "<color=#0FF>On</color>" 
            : "<color=#F00>Off</color>";
    }


    public override void Execute()
    {
        if (!canBeInteracted) return;
        
        base.Execute();
        portal.ToggleActive();
        active = !active;
    }
}
