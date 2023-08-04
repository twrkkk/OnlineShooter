using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;
    private ShootInfo _shootInfo;
    public ShootInfo ShootInfo => _shootInfo;

    public bool TryShoot()
    {
        if (Time.time - _lastShootTime < _shootDelay) return false;

        _lastShootTime = Time.time;
        var position = _bulletSpawn.position;
        var velocity = _bulletSpawn.forward * _bulletSpeed;
        Instantiate(_bullet, position, _bulletSpawn.rotation)
            .Init(velocity);

        SetShootInfo(position, velocity);

        onShoot?.Invoke();

        return true;
    }

    private void SetShootInfo(Vector3 position, Vector3 velocity)
    {
        _shootInfo = new ShootInfo();
        _shootInfo.pos = position.ToV3();
        _shootInfo.vel = velocity.ToV3();
    }
}
