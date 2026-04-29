using UnityEngine;

public class AxisGizmoUI : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationSmooth = 10f;

    void Update()
    {
        // Posición HUD
        transform.position = cameraTransform.position 
                            + cameraTransform.forward * 0.5f 
                            + cameraTransform.right * 0.3f 
                            + cameraTransform.up * -0.3f;

        // Rotación objetivo: mundo fijo
        Quaternion targetRotation = Quaternion.identity;

        // Suavizado
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmooth
        );
    }
}