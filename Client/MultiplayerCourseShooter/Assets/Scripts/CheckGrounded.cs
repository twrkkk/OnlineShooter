using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour
{
    [SerializeField] private float _sphereRadius = 0.4f;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] private float _coyoteTime = 0.1f;
    private float _timer;
    private bool _isGrounded;
    public bool Grounded => _isGrounded;
    void Update()
    {
        if (Physics.CheckSphere(transform.position, _sphereRadius, _layerMask))
        {
            _isGrounded = true;
            _timer = 0f;
        }
        else
        {
            _timer += Time.deltaTime;
            if (_timer > _coyoteTime)
            {
                _isGrounded = false;
            }
        }
    }
}
