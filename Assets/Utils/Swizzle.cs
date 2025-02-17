using UnityEngine;

public static class Swizzle
{
    public static Vector3 X0Y(this Vector2 v)
    {
        return new Vector3(v.x, 0.0f, v.y);
    }
    
    public static Vector3 X0Z(this Vector3 v)
    {
        return new Vector3(v.x, 0.0f, v.z);
    }
    
    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
}
