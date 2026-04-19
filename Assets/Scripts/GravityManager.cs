using UnityEngine;

public static class GravityManager
{
    public static Vector3 worldGravityDir = Vector3.down;
    public static float gravityForce = 9.81f;

    public static Vector3 GetGravity()
    {
        return worldGravityDir * gravityForce;
    }

    public static void ChangeWorldGravity(Vector3 direction)
    {
        worldGravityDir = direction.normalized;
        Debug.Log("Gravedad global: " + worldGravityDir);
    }

    public static void InvertGravity() => worldGravityDir = worldGravityDir*(-1);
}
