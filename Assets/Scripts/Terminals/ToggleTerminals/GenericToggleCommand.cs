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

    protected override string GetTrivialDisplay2Text(){
        string displayText = "";
        foreach(TerminalActivable activable in activables){
            if(activable is Portal)
                if(activable.initialState)
                    displayText += "deactivate: "+activable.name+"\n";
                else
                    displayText += "activate: "+activable.name+"\n";
            if(activable is GravityZone)
                if(activable.initialState)
                    displayText += "deactivate: "+activable.name+"\n";
                else
                    displayText += "activate: "+activable.name+"\n";
        }
        
        return displayText;
    }
}