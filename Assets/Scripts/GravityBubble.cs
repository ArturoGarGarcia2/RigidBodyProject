using Unity.VisualScripting;
using UnityEngine;

public class GravityBubble : MonoBehaviour
{
    public Renderer render;
    public Light baseLight;

    void Update()
    {
        render.material.SetColor("_BaseColor", GravityManager.GetColorFromGravity(GravityManager.worldGravityDir));
        baseLight.color = GravityManager.GetColorFromGravity(GravityManager.worldGravityDir);
    }
}
