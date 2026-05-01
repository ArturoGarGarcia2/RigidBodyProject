using TMPro;
using UnityEngine;

public class GenericToggleCommand : ToggleCommand
{
    [Header ("Target")]
    public TerminalActivable[] activables;

    void Start(){
        foreach (TerminalActivable activable in activables)
            activable.SetActive(activable.initialState);
    }

    protected override void OnToggle()
    {
        foreach (TerminalActivable activable in activables)
            activable.ToggleActive();
    }
}