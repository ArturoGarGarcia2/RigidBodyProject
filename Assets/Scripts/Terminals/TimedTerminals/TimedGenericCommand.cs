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
}
