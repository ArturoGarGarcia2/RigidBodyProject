using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public Camera portalCamera;
    public Camera playerCamera;

    void LateUpdate()
    {
        if (linkedPortal == null || playerCamera == null || portalCamera == null) return;

        UpdateCamera();
    }

    void UpdateCamera()
    {
        portalCamera.nearClipPlane = playerCamera.nearClipPlane;
        portalCamera.farClipPlane = playerCamera.farClipPlane;
        portalCamera.fieldOfView = playerCamera.fieldOfView;
        portalCamera.aspect = playerCamera.aspect;

        Vector3 relativePos = transform.InverseTransformPoint(playerCamera.transform.position);
        
        relativePos = new Vector3(-relativePos.x, relativePos.y, -relativePos.z);
        
        portalCamera.transform.position = linkedPortal.transform.TransformPoint(relativePos);

        Vector3 dir = transform.InverseTransformDirection(playerCamera.transform.forward);
        Vector3 up = transform.InverseTransformDirection(playerCamera.transform.up);

        Vector3 forwardDest = linkedPortal.transform.TransformDirection(new Vector3(-dir.x, dir.y, -dir.z));
        Vector3 upDest = linkedPortal.transform.TransformDirection(new Vector3(-up.x, up.y, -up.z));

        portalCamera.transform.rotation = Quaternion.LookRotation(forwardDest, upDest);

        SetNearClipPlane();
    }

    void SetNearClipPlane()
    {
        Transform clipPlane = linkedPortal.transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, clipPlane.position - portalCamera.transform.position));

        Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, -Vector3.Dot(camSpacePos, camSpaceNormal));

        portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }
}