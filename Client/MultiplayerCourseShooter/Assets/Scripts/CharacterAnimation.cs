using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] CheckGrounded _checkGrounded;
    [SerializeField] private Character _character;
    private const string Grounded = "Grounded";
    private const string Speed = "Speed";

    private void Update()
    {
        SetGrounded();
        SetSpeed();
    }

    private void SetGrounded()
    {
        _animator.SetBool(Grounded, _checkGrounded.Grounded);
    }

    private void SetSpeed()
    {
        var localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
        float speed = localVelocity.magnitude / _character.Speed;
        var sign = Mathf.Sign(localVelocity.z);
        _animator.SetFloat(Speed, sign * speed);
    }
}
