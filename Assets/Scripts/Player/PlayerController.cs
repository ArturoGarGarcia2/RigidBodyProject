using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerController : GravitableObject, IRespawnable
{
    public Animator animator;
    public TextMeshProUGUI debugTxt;
    public static PlayerController Instance { get; private set; }

    [Header("Ajustes de Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float rotationSpeed = 10f;
    public float lookSensitivity = 0.2f;

    [Header("Referencias")]
    public Transform cameraTransform;
    public Transform holdPoint;

    [Header("Interactuadas")]
    public LayerMask grabbableLayer;
    public float grabRange = 5f;
    public float holdForce = 20f;
    public float throwForce = 10f;
    public LayerMask interactLayer;

    [Header("Sprint")]
    public float sprintMultiplier = 1.5f;
    private bool isSprinting = false;
    private bool isInteracting = false;


    Rigidbody grabbedRb;
    Collider grabbedCollider;
    public Light[] gravityLights;


    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraPitch = 0f;
    [SerializeField] GameObject graphicsObject;


    protected override void Awake()
    {
        GravityManager.ChangeWorldGravity(Vector3.down);
        base.Awake();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        animator.applyRootMotion = false;

        traveller = GetComponent<PortalTraveller>();
        traveller.graphicsObject = graphicsObject;

        playerColliders = GetComponentsInChildren<Collider>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- INPUT EVENTS ---
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    public void OnJump()
    {
        if (IsGrounded())
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.Get<float>() > 0.5f;
    }

    // public void OnGravityDown()    => GravityManager.ChangeWorldGravity(Vector3.down);
    // public void OnGravityUp()      => GravityManager.ChangeWorldGravity(Vector3.up);
    // public void OnGravityLeft()    => GravityManager.ChangeWorldGravity(Vector3.left);
    // public void OnGravityRight()   => GravityManager.ChangeWorldGravity(Vector3.right);
    // public void OnGravityForward() => GravityManager.ChangeWorldGravity(Vector3.forward);
    // public void OnGravityBack()    => GravityManager.ChangeWorldGravity(Vector3.back);


    // --- LÓGICA DE FÍSICAS ---
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        ApplyMovement();
        AlignWithCurrentGravity();

        if (grabbedRb != null)
            HoldObject();

        float verticalSpeed = Vector3.Dot(rb.linearVelocity, transform.up);
        animator.SetFloat("VerticalSpeed", verticalSpeed);
        animator.SetBool("IsGrounded", IsGrounded());
    }

    void LateUpdate()
    {
        ApplyLook();
    }

    void Update()
    {
        foreach(Light l in gravityLights)
            if(!useLocalGravity)
                l.color = GravityManager.GetColorFromGravity(GravityManager.worldGravityDir);
            else
                l.color = GravityManager.GetColorFromGravity(localGravityDir);
    }

    private void ApplyMovement()
    {
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        Vector3 targetMoveVelocity = (transform.forward * moveInput.y + transform.right * moveInput.x) * currentSpeed;
        
        float verticalVelocity = Vector3.Dot(rb.linearVelocity, transform.up);
        
        Vector3 platformVelocity = Vector3.zero;
        if (transform.parent != null)
        {
            Rigidbody parentRb = transform.parent.GetComponentInParent<Rigidbody>();
            if (parentRb != null)
                platformVelocity = parentRb.linearVelocity;
        }

        rb.linearVelocity = targetMoveVelocity + platformVelocity + (transform.up * verticalVelocity);

        if (animator != null)
        {
            float animMultiplier = isSprinting ? sprintMultiplier : 1f;
            animator.SetFloat("MoveX", moveInput.x * animMultiplier);
            animator.SetFloat("MoveY", moveInput.y * animMultiplier);
        }
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

    void GrabState(Rigidbody rb, bool state)
    {
        GravitableObject g = rb.GetComponent<GravitableObject>();
        if (g != null)
            g.isHeld = state;
    }

    void SetHeldLayer(GameObject obj, bool held)
    {
        obj.layer = held ? LayerMask.NameToLayer("HeldObject") : LayerMask.NameToLayer("Grabbable");
    }

    void HandleInteract()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            TerminalCommand terminal = hit.collider.GetComponentInParent<TerminalCommand>();

            if (terminal != null)
            {
                terminal.Execute();
                return;
            }

            Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();

            if (rb != null)
                Grab(hit);
        }
    }

    private Collider[] grabbedColliders;
    private bool[] originalIsTrigger;
    private Collider[] playerColliders;
    private Transform originalParent;

    void Grab(RaycastHit hit)
    {
        Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            grabbedRb = rb;
            grabbedCollider = hit.collider;

            originalParent = grabbedRb.transform.parent;
            grabbedRb.transform.SetParent(holdPoint);

            grabbedColliders = grabbedRb.GetComponentsInChildren<Collider>();

            // 🚫 Ignorar colisiones SOLO con el jugador
            foreach (var objCol in grabbedColliders)
            {
                foreach (var playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(objCol, playerCol, true);
                }
            }

            SetHeldLayer(grabbedRb.gameObject, true);
            GrabState(grabbedRb, true);

            // 🔧 evitar empujón inicial
            grabbedRb.position = holdPoint.position;

            grabbedRb.linearVelocity = Vector3.zero;
            grabbedRb.angularVelocity = Vector3.zero;

            grabbedRb.useGravity = false;
            grabbedRb.linearDamping = 10f;

            // 🔒 bloquear rotación
            grabbedRb.constraints = RigidbodyConstraints.FreezeRotation;

            // 🔥 evitar atravesar paredes a velocidad
            grabbedRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            GravityZone zone = grabbedRb.GetComponentInParent<GravityZone>();
            if (zone != null)
            {
                zone.OnTriggerExit(grabbedCollider);
            }
        }
    }

    void Release()
    {
        if (grabbedRb != null)
        {
            GrabState(grabbedRb, false);
            
            grabbedRb.useGravity = false;
            grabbedRb.linearDamping = 0f;

            SetHeldLayer(grabbedRb.gameObject, false);
            grabbedRb.transform.SetParent(originalParent);
            
            grabbedRb.constraints = RigidbodyConstraints.None;
            grabbedRb.freezeRotation = false;

            // 🔄 restaurar colisiones con jugador
            foreach (var objCol in grabbedColliders)
            {
                if (objCol == null) continue;

                foreach (var playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(objCol, playerCol, false);
                }
            }

            // 🔧 volver a modo normal de colisiones
            grabbedRb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            grabbedRb = null;
            grabbedCollider = null;originalParent = null;
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
            if (grabbedRb == null)
                HandleInteract();
            else
                Release();
    }

    void HoldObject()
    {
        Vector3 targetPos = holdPoint.position;
        Vector3 dir = targetPos - grabbedRb.position;

        grabbedRb.position = holdPoint.position;
        grabbedRb.rotation = holdPoint.rotation;

        grabbedRb.linearVelocity = dir * holdForce;
    }

    private bool isReloading = false;

    public void OnOutOfBounds()
    {
        if (isReloading) return;

        isReloading = true;
        ReloadLevel();
    }

    void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
