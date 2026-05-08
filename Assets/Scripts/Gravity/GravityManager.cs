using UnityEngine;

public static class GravityManager
{
    public static Vector3 worldGravityDir = Vector3.down;
    public static float gravityForce = 9.81f;

    public static Vector3 GetGravity() => worldGravityDir * gravityForce;

    public static void ChangeWorldGravity(Vector3 direction) => worldGravityDir = direction.normalized;

    public static string GetGravityText() => "<color="+GetHexadecimalFromGeneralGravity()+">"+GetNameFromGeneralGravity()+"</color>";

    
    public static Color GetColorFromGravity(Vector3 d)
    {
        if (d == Vector3.up) return Color.yellow;
        if (d == Vector3.down) return Color.white;
        if (d == Vector3.left) return Color.green;
        if (d == Vector3.right) return Color.blue;
        if (d == Vector3.forward) return Color.red;
        if (d == Vector3.back) return new Color(1f, .5f, 0f);

        return Color.white;
    }
    
    public static string GetNameFromGeneralGravity()
    {
        if (worldGravityDir == Vector3.up) return "UP";
        if (worldGravityDir == Vector3.down) return "DOWN";
        if (worldGravityDir == Vector3.left) return "LEFT";
        if (worldGravityDir == Vector3.right) return "RIGHT";
        if (worldGravityDir == Vector3.forward) return "FORWARDS";
        if (worldGravityDir == Vector3.back) return "BACKWARDS";

        return "UP";
    }

    public static string GetHexadecimalFromGeneralGravity()
    {
        if (worldGravityDir == Vector3.up) return "#FFF";
        if (worldGravityDir == Vector3.down) return "#FF0";
        if (worldGravityDir == Vector3.left) return "#0F0";
        if (worldGravityDir == Vector3.right) return "#00F";
        if (worldGravityDir == Vector3.forward) return "#F00";
        if (worldGravityDir == Vector3.back) return "#F80";

        return "#FFF";
    }
}
