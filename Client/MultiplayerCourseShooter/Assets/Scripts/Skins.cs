using UnityEngine;

public class Skins : MonoBehaviour
{
    [SerializeField] private Material[] _skins;
    public int Length { get { return _skins.Length; } }
    public Material GetMaterial(int index)
    {
        Material result = null;
        if (Mathf.Abs(index) >= _skins.Length)
            result = _skins[0];
        else
            result = _skins[index];

        return result;
    }
}
