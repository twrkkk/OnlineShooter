using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthUI _healthUI;
    private int _max;
    private int _current;

    public void SetMax(int value)
    {
        _max = value;
        UpdateHealth();
    }

    public void SetCurrent(int value)
    {
        _current = value;
        UpdateHealth();
    }

    public void ApplyDamage(int damage)
    {
        if (_current < 0) return;
        _current -= damage;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        _healthUI.UpdateHealth(_max, _current);
    }
}
