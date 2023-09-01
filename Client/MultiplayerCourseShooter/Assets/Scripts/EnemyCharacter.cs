using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    private string _sessionID;

    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    [SerializeField] private float _rotateLerpVerticalSpeed;
    [SerializeField] private float _rotateLerpHorizontalSpeed;
    [SerializeField] private BodyAnimation _bodyAnimation;
    private Vector3 _targetPosition;
    private Vector2 _targetRotation;

    private float _velocityMagnitude;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
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

    private void Rotate()
    {
        float x = Mathf.LerpAngle(_head.localEulerAngles.x, _targetRotation.x, _rotateLerpVerticalSpeed * Time.deltaTime);
        float y = Mathf.LerpAngle(transform.localEulerAngles.y, _targetRotation.y, _rotateLerpHorizontalSpeed * Time.deltaTime);

        _head.localEulerAngles = new Vector3(x, 0f, 0f);
        transform.localEulerAngles = new Vector3(0f, y, 0f);
    }

    public void Init(string sessionID)
    {
        _sessionID = sessionID; 
    }

    public void SetSpeed(float value) => Speed = value;
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        _health.SetMax(maxHealth);
        _health.SetCurrent(maxHealth);
    }

    public void SetHealth(int value)
    {
        _health.SetCurrent(value);
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float avgDelay)
    {
        _targetPosition = position + velocity * avgDelay;
        Velocity = velocity;
        _velocityMagnitude = velocity.magnitude;
    }
    public void SetRotationX(float value)
    {
        //_head.localEulerAngles = new Vector3(value, 0f, 0f);
        _targetRotation.x = value;
    }

    public void SetRotationY(float value)
    {
        //transform.localEulerAngles = new Vector3(0f, value, 0f);
        _targetRotation.y = value;
    }

    public void SitDown()
    {
        _bodyAnimation.Sitdown(Sitdown);
    }

    public void ApplyDamage(int value)
    {
        Debug.Log(value);
        _health.ApplyDamage(value);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "id", _sessionID},
            {"dmg",  value}
        };

        MultiplayerManager.Instance.SendMessage("dmg", data);
    }
}
