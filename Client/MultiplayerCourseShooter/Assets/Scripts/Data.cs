using System;

[Serializable]
public struct V3
{
    public float x;
    public float y;
    public float z;

    public V3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[Serializable]
public struct ShootInfo
{
    public string key;
    public V3 pos;
    public V3 vel;
}

[Serializable]
public struct RespawnInfo
{
    public sbyte x;
    public sbyte z;

    public RespawnInfo(sbyte x, sbyte z)
    {
        this.x = x;
        this.z = z;
    }
}
