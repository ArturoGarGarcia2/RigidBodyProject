using UnityEngine;
using System.Collections;

public class BatteryGenerator : MonoBehaviour
{
    [SerializeField] private GameObject batteryPrefab;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private float respawnDelay = 2f;

    private GameObject currentBattery;
    private bool isRespawning = false;

    void Start()
    {
        StartCoroutine(RespawnBattery());
    }

    void Update()
    {
        if (currentBattery == null && !isRespawning)
            StartCoroutine(RespawnBattery());
    }

    IEnumerator RespawnBattery()
    {
        isRespawning = true;

        yield return new WaitForSeconds(respawnDelay);

        SpawnBattery();

        isRespawning = false;
    }

    void SpawnBattery() => currentBattery = Instantiate(
            batteryPrefab,
            generationPoint.position,
            Quaternion.identity
        );
}