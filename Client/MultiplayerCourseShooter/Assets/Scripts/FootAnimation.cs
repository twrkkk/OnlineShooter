using UnityEngine;

public class FootAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] CheckGrounded _checkGrounded;
    [SerializeField] private Character _character;
    private const string Grounded = "Grounded";
    private const string Speed = "Speed";
    private const string Sit = "Sit";
    private bool _sit;

    private void Update()
    {
        SetGrounded();
        SetSpeed();
        SetSitdown();
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

    private void SetSitdown()
    {
        if(_sit != _character.Sitdown)
        {
            _sit = _character.Sitdown;
            _animator.SetBool(Sit, _sit);
        }
    }
}
