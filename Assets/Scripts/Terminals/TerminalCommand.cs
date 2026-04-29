using TMPro;
using UnityEngine;

public class TerminalCommand : MonoBehaviour
{
    public bool canBeInteracted;
    public TMP_Text display;
    public virtual void Execute(){}
    public virtual void Update()
    {
        if(!canBeInteracted)
            display.text = "<color=#F00>No\nService</color>";
    }
}
