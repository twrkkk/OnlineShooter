using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    public void Init(Vector3 direction, float speed)
    {
        _rb.velocity = direction * speed;
    }
}
