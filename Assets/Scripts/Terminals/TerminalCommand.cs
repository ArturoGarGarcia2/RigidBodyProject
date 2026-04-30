using TMPro;
using UnityEngine;

public abstract class TerminalCommand : MonoBehaviour
{
    public bool canBeInteracted;
    [SerializeField] protected TMP_Text display;

    public virtual void Execute(){}

    protected virtual string GetDisplayText()
    {
        return "<color=#F00>No\nService</color>";
    }

    public virtual void Update()
    {
        display.text = canBeInteracted 
            ? GetDisplayText() 
            : "<color=#F00>No\nService</color>";
    }
}