using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public Direction dir; // El enum que ya tienes creado

    private void OnTriggerEnter(Collider other)
    {
        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
        {
            gravObj.ChangeGravity(GetVectorFromEnum(dir));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
        {
            gravObj.ResetToWorldGravity();
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
}