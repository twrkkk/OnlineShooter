using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitBox : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 1f;
    public float DamageMultiplier => _damageMultiplier;
}
