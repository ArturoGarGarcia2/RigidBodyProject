using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : GravitableObject
{
    [Header("Ajustes de Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float rotationSpeed = 10f;
    public float lookSensitivity = 0.2f;

    [Header("Referencias")]
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraPitch = 0f;

    protected override void Awake()
    {
        base.Awake();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }


    // --- INPUT EVENTS ---
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    public void OnJump()
    {
        if (IsGrounded())
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void OnGravityDown()    => GravityManager.ChangeWorldGravity(Vector3.down);
    public void OnGravityUp()      => GravityManager.ChangeWorldGravity(Vector3.up);
    public void OnGravityLeft()    => GravityManager.ChangeWorldGravity(Vector3.left);
    public void OnGravityRight()   => GravityManager.ChangeWorldGravity(Vector3.right);
    public void OnGravityForward() => GravityManager.ChangeWorldGravity(Vector3.forward);
    public void OnGravityBack()    => GravityManager.ChangeWorldGravity(Vector3.back);


    // --- LÓGICA DE FÍSICAS ---
    protected override void FixedUpdate()
    {
        base.FixedUpdate(); // Aplica la fuerza de gravedad personalizada del padre
        
        ApplyMovement();
        AlignWithCurrentGravity();
    }

    void LateUpdate()
    {
        ApplyLook();
    }

    private void ApplyMovement()
    {
        Vector3 targetVelocity = (transform.forward * moveInput.y + transform.right * moveInput.x) * moveSpeed;
        float verticalVelocity = Vector3.Dot(rb.linearVelocity, transform.up);
        rb.linearVelocity = targetVelocity + (transform.up * verticalVelocity);
    }

    private void AlignWithCurrentGravity()
    {
        Vector3 gravityDir = useLocalGravity 
            ? localGravityDir 
            : GravityManager.worldGravityDir;

        Vector3 targetUp = -gravityDir;

        Vector3 forwardProjected = Vector3.ProjectOnPlane(cameraTransform.forward, targetUp);

        if (forwardProjected.sqrMagnitude < 0.001f)
            forwardProjected = Vector3.ProjectOnPlane(transform.forward, targetUp);

        Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, targetUp);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
    }

    private void ApplyLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity, Space.Self);

        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }

    private bool IsGrounded() => Physics.Raycast(transform.position, -transform.up, 1.1f);
}