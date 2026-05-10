using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class DimensionRoom : MonoBehaviour
{
    [System.Serializable]
    public class TrackedObject
    {
        public DimensionalEchoer traveller;
        public Vector3 localPosition;
    }

    public DimensionType dimension;
    public DimensionRoom oppositeRoom;

    public Transform roomRoot;

    public List<TrackedObject> objectsInside = new List<TrackedObject>();

    void Update()
    {
        foreach (var obj in objectsInside)
        {
            DimensionalEchoer t = obj.traveller;

            if (t == null) continue;
            if (t.isEcho) continue;
            if (t.manifestationPending) continue;

            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb == null) continue;

            bool isStopped = rb.linearVelocity.magnitude < 0.05f;

            if (isStopped)
            {
                if (t.spawnedEcho != null) continue;
                if (t.GetComponent<GenericGravityBody>() != null &&
                    t.GetComponent<GenericGravityBody>().isHeld)
                    continue;

                t.manifestationPending = true;

                StartCoroutine(
                    HandleManifestation(
                        () => SpawnEcho(t),
                        t,
                        0.5f
                    )
                );
            }
            else
            {
                if (t.spawnedEcho == null) continue;

                StartCoroutine(
                    HandleManifestation(
                        () => DespawnEcho(t),
                        t,
                        0.1f
                    )
                );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DimensionalEchoer t = other.GetComponentInParent<DimensionalEchoer>();

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
        DimensionalEchoer t = other.GetComponentInParent<DimensionalEchoer>();

        if (t == null) return;

        TrackedObject entry = Get(t);

        if (entry != null)
        {
            entry.localPosition = transform.InverseTransformPoint(t.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DimensionalEchoer t = other.GetComponentInParent<DimensionalEchoer>();

        if (t != null)
        {
            objectsInside.RemoveAll(x => x.traveller == t);
        }
    }

    bool Contains(DimensionalEchoer t)
        => objectsInside.Exists(x => x.traveller == t);

    TrackedObject Get(DimensionalEchoer t)
        => objectsInside.Find(x => x.traveller == t);

    // ----------------------------
    // SPAWN / DESPAWN
    // ----------------------------

    void SpawnEcho(DimensionalEchoer t)
    {
        if (t == null) return;
        if (t.isEcho) return;
        if (t.spawnedEcho != null) return;

        GameObject prefabToSpawn = null;

        if (dimension == DimensionType.Ship)
            prefabToSpawn = t.dungeonEchoPrefab;
        else
            prefabToSpawn = t.shipEchoPrefab;

        if (prefabToSpawn == null || oppositeRoom == null) return;

        Vector3 localPos =
            transform.InverseTransformPoint(t.transform.position);

        Vector3 targetPos =
            oppositeRoom.roomRoot.TransformPoint(localPos);

        GameObject echo = Instantiate(
            prefabToSpawn,
            targetPos,
            t.transform.rotation
        );

        DimensionalEchoer echoer = echo.GetComponent<DimensionalEchoer>();

        if (echoer != null)
        {
            echoer.isEcho = true;
        }

        t.spawnedEcho = echo;
    }

    void DespawnEcho(DimensionalEchoer t)
    {
        if (t == null) return;

        if (t.spawnedEcho != null)
        {
            Destroy(t.spawnedEcho);
            t.spawnedEcho = null;
        }
    }

    // ----------------------------
    // COROUTINE
    // ----------------------------

    private IEnumerator HandleManifestation(
        Action action,
        DimensionalEchoer traveller,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        if (traveller == null)
            yield break;

        traveller.manifestationPending = false;

        action?.Invoke();
    }
}