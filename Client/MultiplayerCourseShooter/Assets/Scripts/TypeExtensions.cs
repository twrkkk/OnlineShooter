using UnityEngine;

public static class TypeExtensions
{
    public static V3 ToV3(this Vector3 vector) =>
        new V3(vector.x, vector.y, vector.z);
}
