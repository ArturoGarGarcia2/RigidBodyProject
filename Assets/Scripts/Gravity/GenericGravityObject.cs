using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericGravityBody : GravitableObject, IRespawnable
{

    [Header("Restricciones de Movimiento")]
    public bool allowX = true;
    public bool allowY = true;
    public bool allowZ = true;

    protected override void FixedUpdate()
    {
        ApplyFilteredGravity();
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

    public void OnOutOfBounds()
    {
        Destroy(gameObject);
    }
}