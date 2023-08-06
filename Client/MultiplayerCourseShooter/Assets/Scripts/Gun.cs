using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Bullet _bullet;
    public Action onShoot;
}
