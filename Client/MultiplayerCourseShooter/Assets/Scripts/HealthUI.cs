using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defaultWidth;

    private void OnValidate()
    {
        _defaultWidth = _filledImage.rect.width;
    }
    
    public void UpdateHealth(int max, int current)
    {
        float percentage = (float) current / max;

        _filledImage.sizeDelta = new Vector2(_defaultWidth * percentage, _filledImage.sizeDelta.y);
    }
}
