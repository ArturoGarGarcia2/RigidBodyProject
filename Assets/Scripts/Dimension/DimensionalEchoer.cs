using UnityEngine;

public class DimensionalEchoer : MonoBehaviour
{
    [Header("Echo Prefabs")]
    public GameObject shipEchoPrefab;
    public GameObject dungeonEchoPrefab;

    [HideInInspector] public GameObject spawnedEcho;
    [HideInInspector] public bool manifestationPending;

    void Awake()
    {
        // Si ya es un eco, no debe volver a generar otro eco
        if (isEcho)
        {
            manifestationPending = false;
        }
    }

    public bool isEcho = false;

    public GameObject GetEchoPrefab(DimensionType fromRoom)
    {
        if (fromRoom == DimensionType.Ship)
            return dungeonEchoPrefab;

        if (fromRoom == DimensionType.Dungeon)
            return shipEchoPrefab;

        return null;
    }

    public bool CanSpawnEcho(Rigidbody rb)
    {
        if (isEcho) return false;
        if (spawnedEcho != null) return false;
        if (rb == null) return false;

        return rb.linearVelocity.magnitude < 0.05f;
    }
}