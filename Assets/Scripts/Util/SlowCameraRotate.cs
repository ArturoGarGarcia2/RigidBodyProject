using UnityEngine;

public class SlowCameraRotate : MonoBehaviour
{
    [Header("Rotación")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Grados por segundo")]
    public float rotationSpeed = 10f;

    [Header("Espacio")]
    public bool useLocalSpace = true;

    void Update()
    {
        Space space = useLocalSpace ? Space.Self : Space.World;

        transform.Rotate(
            rotationAxis.normalized * rotationSpeed * Time.deltaTime,
            space
        );
    }
}