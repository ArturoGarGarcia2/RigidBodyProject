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
        display.text = active ? "On" : "Off";
        display.color = active ? Color.green : Color.red;
    }


    public override void Execute()
    {
        base.Execute();
        portal.ToggleActive();
        active = !active;
    }
}
