using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; protected set; } = 100;
    [field: SerializeField] public float Speed { get; protected set; }
    public Vector3 Velocity { get; protected set; }
    public bool Sitdown;
}

