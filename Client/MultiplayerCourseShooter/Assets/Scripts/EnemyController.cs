using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _delayCount = 5;
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun[] _guns;
    private Vector3 _targetPosition;
    private List<float> _delays = new List<float>();
    private float _lastReceiveTime;
    private Player _player;

    private int _currGunIndex;
    private EnemyGun _currGun;
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

    public void Init(string key, Player player)
    {
        _character.Init(key);
        _player = player;
        _player.OnChange += OnChange;

        _character.SetSpeed(player.speed);
        _character.SetMaxHealth(player.maxHP);
        ChangeGun(0);
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = info.pos.ToVector3();
        Vector3 velocity = info.vel.ToVector3();
        _currGun.Shoot(position, velocity);
    }

    public void Sit(in SitdownInfo info)
    {
        _character.Sitdown = info.sit;
        _character.SitDown();
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
                case "loss":
                    MultiplayerManager.Instance.LossCounter.SetEnemyLoss((byte)change.Value);
                    break;
                case "curHP":
                    if ((sbyte)change.Value > (sbyte)change.PreviousValue)
                        _character.SetHealth((sbyte)change.Value);
                    break;
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

    public void ChangeGun(int index)
    {
        if (GunIndexOutOfRange(index)) return;
        _currGunIndex = index;
        _currGun = _guns[_currGunIndex];

        for (int i = 0; i < _guns.Length; i++)
        {
            if (i == _currGunIndex)
                _guns[i].gameObject.SetActive(true);
            else
                _guns[i].gameObject.SetActive(false);
        }
    }

    private bool GunIndexOutOfRange(int index)
    {
        return index < 0 || index > _guns.Length - 1;
    }
}
