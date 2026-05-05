using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Transform player;

    [Header("Ajustes")]
    public float openDistance = 4f;
    public bool canOpen = false; // lo puedes controlar desde fuera (energía, etc.)
    public GameObject[] displays;

    private bool playerNearby = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Auto-buscar jugador si no está asignado
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        bool playerNearby = distance <= openDistance;

        animator.SetBool("character_nearby", playerNearby);
        animator.SetBool("can_open", canOpen);

        foreach(GameObject display in displays)
            display.SetActive(!canOpen);
    }

    // Para tu sistema externo (energía, etc.)
    public void SetCanOpen(bool value)
    {
        canOpen = value;
    }
}