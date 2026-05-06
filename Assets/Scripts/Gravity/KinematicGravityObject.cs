using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KinematicGravityBody : MonoBehaviour, IRespawnable
{
    private Rigidbody rb;

    [Header("Gravedad")]
    public bool useLocalGravity = false;
    public Vector3 localGravityDir = Vector3.down;
    public float gravityForce = 9.81f;
    public float maxSpeed = 20f;

    [Header("Restricciones de Movimiento")]
    public bool allowX = true;
    public bool allowY = true;
    public bool allowZ = true;

    private Vector3 velocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        ApplyKinematicGravity();
    }

    private void ApplyKinematicGravity()
    {
        Vector3 gravityDir = useLocalGravity 
            ? localGravityDir 
            : GravityManager.worldGravityDir;

        Vector3 gravity = gravityDir * gravityForce;

        Vector3 mask = new Vector3(
            allowX ? 1f : 0f,
            allowY ? 1f : 0f,
            allowZ ? 1f : 0f
        );

        Vector3 filteredGravity = Vector3.Scale(gravity, mask);

        // Integración
        velocity += filteredGravity * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        velocity = Vector3.Scale(velocity, mask);

        Vector3 movement = velocity * Time.fixedDeltaTime;

        // 🔥 CLAVE: detectar colisión antes de mover
        if (rb.SweepTest(movement.normalized, out RaycastHit hit, movement.magnitude))
        {
            // Nos quedamos justo antes de la colisión
            float safeDistance = hit.distance - 0.01f;
            if (safeDistance > 0f)
                rb.MovePosition(rb.position + movement.normalized * safeDistance);

            // 🔥 MUY IMPORTANTE: cancelar velocidad en esa dirección
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
        else
            rb.MovePosition(rb.position + movement);
    }

    public void ChangeGravity(Vector3 newDir)
    {
        useLocalGravity = true;
        localGravityDir = newDir.normalized;
        velocity = Vector3.zero;
    }

    public void ResetToWorldGravity()
    {
        useLocalGravity = false;
        velocity = Vector3.zero;
    }

    public void OnOutOfBounds()
    {
        Destroy(gameObject);
    }
}