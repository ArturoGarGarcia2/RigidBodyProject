using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericGravityBody : GravitableObject
{
    [Header("Restricciones de Movimiento")]
    public bool allowX = true;
    public bool allowY = true;
    public bool allowZ = true;

    [Header("Rotación automática")]
    public bool alignToGravity = false;
    public float rotationSpeed = 5f;

    protected override void FixedUpdate()
    {
        // Sobrescribimos el comportamiento para filtrar la gravedad según los bools
        ApplyFilteredGravity();

        if (alignToGravity)
        {
            AlignToGravity();
        }
    }

    private void ApplyFilteredGravity()
    {
        Vector3 rawGravity = GetCurrentGravityDir() * gravityForce;
        
        // Creamos la máscara: 1 si está marcado, 0 si no
        Vector3 mask = new Vector3(allowX ? 1f : 0f, allowY ? 1f : 0f, allowZ ? 1f : 0f);
        
        // Multiplicamos componente a componente
        Vector3 filteredGravity = Vector3.Scale(rawGravity, mask);

        rb.AddForce(filteredGravity, ForceMode.Acceleration);
    }

    private void AlignToGravity()
    {
        Vector3 gravityDir = GetCurrentGravityDir();
        
        // Aplicamos la máscara también a la dirección de rotación 
        // para que no intente rotar hacia un eje bloqueado
        Vector3 mask = new Vector3(allowX ? 1f : 0f, allowY ? 1f : 0f, allowZ ? 1f : 0f);
        Vector3 filteredDir = Vector3.Scale(gravityDir, mask);

        if (filteredDir.sqrMagnitude > 0.001f) // Evitar error si todos los ejes son false
        {
            Vector3 targetUp = -filteredDir.normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }
    }
}