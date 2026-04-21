using UnityEngine;

public static class PortalUtils
{
    public static Vector3 TransformPosition(Transform obj, Transform from, Transform to)
    {
        Vector3 localPos = from.InverseTransformPoint(obj.position);
        return to.TransformPoint(localPos);
    }

    public static Vector3 TransformDirection(Vector3 dir, Transform from, Transform to)
    {
        Vector3 localDir = from.InverseTransformDirection(dir);
        return to.TransformDirection(localDir);
    }

    public static Quaternion TransformRotation(Quaternion rot, Transform from, Transform to)
    {
        Quaternion localRot = Quaternion.Inverse(from.rotation) * rot;
        return to.rotation * localRot;
    }
}