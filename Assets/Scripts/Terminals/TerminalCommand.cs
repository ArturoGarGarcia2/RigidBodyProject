using TMPro;
using UnityEngine;

public abstract class TerminalCommand : MonoBehaviour
{
    public bool canBeInteracted = true;

    [Header ("Display")]
    [SerializeField] protected TMP_Text display;
    [SerializeField] protected TMP_Text timeDisplay;
    [SerializeField] protected Renderer batteryDisplay;
    [SerializeField] protected Material lowBattery;
    [SerializeField] protected Material fullBattery;
    [SerializeField] protected GameObject light;

    [Header ("Trivial Display")]
    [SerializeField] protected TMP_Text trivialDisplay1;
    [SerializeField] protected TMP_Text trivialDisplay2;



    protected virtual void Start()
    {
        RefreshDisplay();
    }

    public void SetInteractable(bool value)
    {
        canBeInteracted = value;
        RefreshDisplay();
    }

    public void TryExecute()
    {
        if (!canBeInteracted) return;
        Execute();
        RefreshDisplay();
    }

    protected void Update(){
        batteryDisplay.material = canBeInteracted ? fullBattery : lowBattery;
        RefreshDisplay();
    }

    public abstract void Execute();

    protected virtual string GetDisplayText() =>
        "<size=50%><color=#F00>Out of\nservice</color></size>";

    protected virtual void EnableLight(){
        light.SetActive(canBeInteracted);
    }

    protected void RefreshDisplay()
    {
        if(canBeInteracted)
            EnableLight();
        else
            light.SetActive(canBeInteracted);

        display.text = canBeInteracted
            ? GetDisplayText()
            : "<size=50%><color=#F00>Out of\nservice</color></size>";
        
        trivialDisplay1.text = canBeInteracted
            ? "<color=#999>current_gravity: </color><color="+GravityManager.GetHexadecimalFromGravity()+">"+GravityManager.GetNameFromGravity()+"</color>"
            : "";

        trivialDisplay2.text = canBeInteracted
            ? ""
            // ? "<color=#999>current_gravity: </color><color="+GravityManager.GetHexadecimalFromGravity()+">"+GravityManager.GetNameFromGravity()+"</color>"
            : "";
    }
}