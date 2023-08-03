using System;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;
    private Vector3 _targetPosition;

    private float _velocityMagnitude;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_velocityMagnitude > Constants.Epsilon)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        }
        else
        {
            transform.position = _targetPosition;
        }
    }

    public void SetSpeed(float value) => Speed = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float avgDelay)
    {
        _targetPosition = position + velocity * avgDelay;
        Velocity = velocity;
        _velocityMagnitude = velocity.magnitude;
    }
    public void SetRotationX(float value)
    {
        _head.localEulerAngles = new Vector3(value, 0f, 0f);
    }

    public void SetRotationY(float value)
    {
        transform.localEulerAngles = new Vector3(0f, value, 0f);
    }
}
