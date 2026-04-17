using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitableObject : MonoBehaviour
{
    protected Rigidbody rb;

    [Header("Gravedad")]
    public bool useLocalGravity = false;
    public Vector3 localGravityDir = Vector3.down;
    public float gravityForce = 9.81f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected virtual void FixedUpdate()
    {
        Vector3 gravityDir = useLocalGravity 
            ? localGravityDir 
            : GravityManager.worldGravityDir;

        rb.AddForce(gravityDir * gravityForce, ForceMode.Acceleration);
    }

    public Vector3 GetCurrentGravityDir() => useLocalGravity 
        ? localGravityDir 
        : GravityManager.worldGravityDir;

    public void ChangeGravity(Vector3 newDir)
    {
        useLocalGravity = true;
        localGravityDir = newDir.normalized;

        // Limpieza de velocidad
        rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, localGravityDir);

        // Empujón opcional
        rb.AddForce(localGravityDir * 5f, ForceMode.Impulse);
    }

    public void ResetToWorldGravity()
    {
        useLocalGravity = false;

        Vector3 worldDir = GravityManager.worldGravityDir;

        rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, worldDir);
    }
}