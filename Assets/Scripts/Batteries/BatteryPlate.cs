using UnityEngine;

public class BatteryPlate : MonoBehaviour
{
    private int batteries;
    public int batteriesRequired = 1;
    private bool active;
    [SerializeField] TerminalCommand[] terminals;
    [SerializeField] DoorManager[] doors;
    [SerializeField] protected Renderer batteryDisplay;
    [SerializeField] protected Material lowBattery;
    [SerializeField] protected Material fullBattery;

    void Update()
    {
        // if(terminals.Length == 0) return;
        active = batteries >= batteriesRequired;
        batteryDisplay.material = active ? fullBattery : lowBattery;
        ActiveTerminals();
    }

    private void ActiveTerminals()
    {
        foreach(TerminalCommand terminal in terminals)
            terminal.canBeInteracted = active;
            
        foreach(DoorManager door in doors)
            door.canOpen = active;
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

    void OnValidate()
    {
        if(terminals.Length == 0) return;
        foreach(TerminalCommand terminal in terminals)
            terminal.canBeInteracted = false;

        if(doors.Length == 0) return;
        foreach(DoorManager door in doors)
            door.canOpen = false;
    }
}
