using UnityEngine;

public static class TypeExtensions
{
    public static V3 ToV3(this Vector3 vector) =>
        new V3(vector.x, vector.y, vector.z);

    public static Vector3 ToVector3(this V3 vector) =>
        new Vector3(vector.x, vector.y, vector.z);
}
