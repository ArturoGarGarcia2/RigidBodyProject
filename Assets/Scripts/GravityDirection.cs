using UnityEngine;

[System.Serializable]
public class GravityDirection
{
    // Esto aparecerá como un desplegable en el Inspector de Unity
    public Direction dir;

    public GravityDirection(Direction dir)
    {
        this.dir = dir;
    }

    // Esta función devuelve el Vector3 correspondiente
    public Vector3 GetVector()
    {
        switch (dir)
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

public enum Direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
    FORWARD,
    BACK
}
