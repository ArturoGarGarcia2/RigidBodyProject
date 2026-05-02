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
    
    public string GetGravityText() => "<color="+GetHexadecimal()+">"+GetName()+"</color>";
    
    public string GetName()
    {
        if (dir == Direction.UP) return "UP";
        if (dir == Direction.DOWN) return "DOWN";
        if (dir == Direction.LEFT) return "LEFT";
        if (dir == Direction.RIGHT) return "RIGHT";
        if (dir == Direction.FORWARD) return "FORWARDS";
        if (dir == Direction.BACK) return "BACKWARDS";

        return "UP";
    }

    public string GetHexadecimal()
    {
        if (dir == Direction.UP) return "#FFF";
        if (dir == Direction.DOWN) return "#FF0";
        if (dir == Direction.LEFT) return "#0F0";
        if (dir == Direction.RIGHT) return "#00F";
        if (dir == Direction.FORWARD) return "#F00";
        if (dir == Direction.BACK) return "#F80";

        return "#FFF";
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
