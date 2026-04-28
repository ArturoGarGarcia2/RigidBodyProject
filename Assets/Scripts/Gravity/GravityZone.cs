using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public Direction dir;
    public Renderer[] gravityFieldRenderers;
    public Transform parent;
    public bool mantainGravity;

    void Start()
    {
        foreach (Renderer renderer in gravityFieldRenderers)
        {
            Color c = GetColorFromEnum(dir);

            renderer.material.SetColor("_BaseColor", c);
            renderer.material.SetColor("_EmissionColor", c * 2f); // potencia
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
        {
            gravObj.ChangeGravity(GetVectorFromEnum(dir));

            if(parent != null)
                other.transform.SetParent(parent);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
            gravObj.useLocalGravity = true;
    }

    public void OnTriggerExit(Collider other)
    {
        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
        {
            if (!mantainGravity)
            {
                gravObj.ResetToWorldGravity();
                gravObj.useLocalGravity = false;
            }

            if(parent != null)
                other.transform.SetParent(null);
        }
    }

    private Vector3 GetVectorFromEnum(Direction d) { 
        switch (d)
        {
            case Direction.UP:      return Vector3.up;
            case Direction.DOWN:    return Vector3.down;
            case Direction.LEFT:    return Vector3.left;
            case Direction.RIGHT:   return Vector3.right;
            case Direction.FORWARD: return Vector3.forward;
            case Direction.BACK:    return Vector3.back;
            default:                return Vector3.down; 
        }
    }

    private Color GetColorFromEnum(Direction d) { 
        switch (d)
        {
            case Direction.UP:      return Color.yellow;
            case Direction.DOWN:    return Color.white;
            case Direction.LEFT:    return Color.green;
            case Direction.RIGHT:   return Color.blue;
            case Direction.FORWARD: return Color.red;
            case Direction.BACK:    return new Color(1f, .5f, 0f);
            default:                return Color.white; 
        }
    }
}