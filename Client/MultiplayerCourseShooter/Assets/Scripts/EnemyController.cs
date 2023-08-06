using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _delayCount = 5;
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _gun;
    private Vector3 _targetPosition;
    private List<float> _delays = new List<float>();
    private float _lastReceiveTime;
    private Player _player;
    private float _avgDelay
    {
        get
        {
            float sum = 0f;
            foreach (var delay in _delays)
                sum += delay;
            sum /= _delays.Count;

            return sum;
        }
    }

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    public void Init(Player player)
    {
        _player = player;
        _player.OnChange += OnChange;

        _character.SetSpeed(player.speed);
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = info.pos.ToVector3();
        Vector3 velocity = info.vel.ToVector3();
        _gun.Shoot(position, velocity);
    }

    public void Sit(in SitdownInfo info)
    {
        _character.Sitdown = info.sit;
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SetReceiveTime()
    {
        float delay = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _delays.Add(delay);
        if (_delays.Count > _delayCount)
            _delays.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        Vector3 _velocity = _character.Velocity;
        SetReceiveTime();

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "pX":
                    _targetPosition.x = (float)change.Value;
                    break;
                case "pY":
                    _targetPosition.y = (float)change.Value;
                    break;
                case "pZ":
                    _targetPosition.z = (float)change.Value;
                    break;
                case "vX":
                    _velocity.x = (float)change.Value;
                    break;
                case "vY":
                    _velocity.y = (float)change.Value;
                    break;
                case "vZ":
                    _velocity.z = (float)change.Value;
                    break;
                case "rX":
                    _character.SetRotationX((float)change.Value);
                    break;
                case "rY":
                    _character.SetRotationY((float)change.Value);
                    break;
                default:
                    break;
            }
        }

        _character.SetMovement(_targetPosition, _velocity, _avgDelay);
    }
}
