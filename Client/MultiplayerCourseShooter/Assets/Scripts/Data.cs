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
public struct ChangeGunInfo
{
    public string key;
    public int index;

    public ChangeGunInfo(string key, int index)
    {
        this.key = key;
        this.index = index;
    }
}
