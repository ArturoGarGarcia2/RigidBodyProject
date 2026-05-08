using UnityEngine;

public class TimeTraveller : MonoBehaviour
{
    public TimeState currentState;

    public GameObject futurePrefab;
    private GameObject futureInstance;
    public GameObject spawnedFuture;
    public bool manifestationPending = false;

    public bool hasCreatedFuture = false;

    void Awake()
    {
        if (currentState == TimeState.Future)
        {
            // desactivar auto-spawn de futuros
            hasCreatedFuture = true;
        }
    }

    public virtual void SetRoomState(TimeState state)
    {
        currentState = state;

        if (state == TimeState.Past)
            TryCreateFuture();
    }

    void TryCreateFuture()
    {
        if (hasCreatedFuture) return;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null) return;

        // solo si está quieta
        if (rb.linearVelocity.magnitude > 0.05f) return;

        hasCreatedFuture = true;

        Vector3 futurePos = transform.position; 
        Quaternion futureRot = transform.rotation;

        futureInstance = Instantiate(gameObject, futurePos, futureRot);

        TimeTraveller t = futureInstance.GetComponent<TimeTraveller>();

        if (t != null)
        {
            t.currentState = TimeState.Future;
            t.hasCreatedFuture = true;
        }
    }

    public GameObject GetFuturePrefab() => futurePrefab;
}