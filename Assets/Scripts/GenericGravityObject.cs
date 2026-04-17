using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericGravityBody : GravitableObject
{
    [Header("Rotación automática")]
    public bool alignToGravity = true;
    public float rotationSpeed = 5f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (alignToGravity)
        {
            AlignToGravity();
        }
    }

    private void AlignToGravity()
    {
        Vector3 gravityDir = GetCurrentGravityDir();
        Vector3 targetUp = -gravityDir;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
    }
}