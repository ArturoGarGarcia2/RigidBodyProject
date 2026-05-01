using UnityEngine;

public class BatteryPlateManager : MonoBehaviour
{
    private int batteries;
    public int batteriesRequired;
    private bool active;
    [SerializeField] TerminalCommand[] terminals;

    void Update()
    {
        if(terminals.Length == 0) return;
        active = batteries >= batteriesRequired;
        ActiveTerminals();
    }

    private void ActiveTerminals()
    {
        foreach(TerminalCommand terminal in terminals)
            terminal.canBeInteracted = active;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        batteries++;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Battery"))
        batteries--;
    }

    // void OnValidate()
    // {
    //     if(terminals.Length == 0) return;
    //     foreach(TerminalCommand terminal in terminals)
    //         terminal.canBeInteracted = false;
    // }
}
