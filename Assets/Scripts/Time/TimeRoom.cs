using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class TimeRoom : MonoBehaviour
{
    [System.Serializable]
    public class TrackedObject
    {
        public TimeTraveller traveller;
        public Vector3 localPosition;
    }

    public List<TrackedObject> objectsInside = new List<TrackedObject>();
    [SerializeField] private GameObject futureRoom;
    [SerializeField] private Transform futureRoomRoot;

    void Update()
    {
        foreach (var obj in objectsInside)
        {
            if(obj.traveller.currentState == TimeState.Future) continue;

            Rigidbody rb = obj.traveller.GetComponent<Rigidbody>();

            if (rb == null) continue;

            if (rb.linearVelocity.magnitude < 0.05f)
            {
                if(obj.traveller.hasCreatedFuture) continue;
                if(obj.traveller.manifestationPending) continue;
                if(obj.traveller.GetComponent<GenericGravityBody>().isHeld) continue;

                obj.traveller.manifestationPending = true;

                StartCoroutine(
                    HandleTimeManifestation(
                        () => SpawnFuture(obj.traveller),
                        obj.traveller,
                        .5f
                    )
                );
            }
            else
            {
                if(!obj.traveller.hasCreatedFuture)
                    continue;

                StartCoroutine(
                    HandleTimeManifestation(
                        () => DespawnFuture(obj.traveller),
                        obj.traveller,
                        .1f
                    )
                );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TimeTraveller t = other.GetComponentInParent<TimeTraveller>();

        if (t != null && !Contains(t))
        {
            TrackedObject entry = new TrackedObject();
            entry.traveller = t;
            entry.localPosition = transform.InverseTransformPoint(t.transform.position);

            objectsInside.Add(entry);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        TimeTraveller t = other.GetComponentInParent<TimeTraveller>();

        if (t == null) return;

        TrackedObject entry = Get(t);

        if (entry != null)
        {
            entry.localPosition = transform.InverseTransformPoint(t.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TimeTraveller t = other.GetComponentInParent<TimeTraveller>();

        if (t != null)
        {
            objectsInside.RemoveAll(x => x.traveller == t);
        }
    }

    bool Contains(TimeTraveller t)
    {
        return objectsInside.Exists(x => x.traveller == t);
    }

    TrackedObject Get(TimeTraveller t)
    {
        return objectsInside.Find(x => x.traveller == t);
    }

    void SpawnFuture(TimeTraveller t)
    {
        if (t.futurePrefab == null) return;
        if (t.spawnedFuture != null) return;

        Debug.Log("Tiene un prefab futuro");

        Vector3 localPos = transform.InverseTransformPoint(t.transform.position);
        Vector3 futureWorldPos = futureRoomRoot.TransformPoint(localPos);

        GameObject futureObj = Instantiate(
            t.futurePrefab,
            futureWorldPos,
            t.transform.rotation
        );

        t.spawnedFuture = futureObj;
        t.hasCreatedFuture = true;

        TimeTraveller ft = futureObj.GetComponent<TimeTraveller>();

        if (ft != null)
        {
            ft.currentState = TimeState.Future;
            ft.hasCreatedFuture = true;
        }
    }

    void DespawnFuture(TimeTraveller t)
    {
        if (t.spawnedFuture != null)
        {
            Destroy(t.spawnedFuture);
            t.spawnedFuture = null;
        }

        t.hasCreatedFuture = false;
    }

    private IEnumerator HandleTimeManifestation(
        Action action,
        TimeTraveller traveller,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        if (traveller == null)
            yield break;

        traveller.manifestationPending = false;

        action?.Invoke();
    }
}