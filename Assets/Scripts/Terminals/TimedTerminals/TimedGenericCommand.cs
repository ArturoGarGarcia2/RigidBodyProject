using UnityEngine;

public class TimedGenericCommand : TimedCommand
{
    [Header ("Target")]
    public TerminalActivable[] activables;

    void Start(){
        foreach(TerminalActivable activable in activables)
            activable.SetActive(activable.initialState);
    }

    protected override void OnStart(){
        foreach(TerminalActivable activable in activables)
            activable.SetActive(!activable.initialState);
    }

    protected override void OnEnd(){
        foreach(TerminalActivable activable in activables)
            activable.SetActive(activable.initialState);
    }

    
    protected override string GetTrivialDisplay2Text(){
        string displayText = "";
        foreach(TerminalActivable activable in activables){
            if(activable is Portal)
                if(activable.initialState)
                    displayText += "deactivating: "+activable.name+"\n";
                else
                    displayText += "activating: "+activable.name+"\n";
            if(activable is GravityZone)
                if(activable.initialState)
                    displayText += "deactivating: "+activable.name+"\n";
                else
                    displayText += "activating: "+activable.name+"\n";
        }
        
        return displayText;
    }
}
