using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    private bool canBePressed;
    private PlayerController pc;

    public void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        canBePressed = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        canBePressed = false;
    }

    void Update()
    {
        if(true) return;
    }
}
