using UnityEngine;

public class BoundsVolume : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var respawnable = other.GetComponent<IRespawnable>();

        if (respawnable != null)
        {
            respawnable.OnOutOfBounds();
        }
    }
}