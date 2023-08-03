using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;
    private ShootInfo _shootInfo;
    public ShootInfo ShootInfo => _shootInfo;

    public Action onShoot;
    public bool TryShoot()
    {
        if (Time.time - _lastShootTime < _shootDelay) return false;

        _lastShootTime = Time.time;
        var position = _bulletSpawn.position;
        var direction = _bulletSpawn.forward;
        Instantiate(_bullet, position, _bulletSpawn.rotation)
            .Init(direction, _bulletSpeed);

        SetShootInfo(position, direction);

        onShoot?.Invoke();

        return true;
    }

    private void SetShootInfo(Vector3 position, Vector3 direction)
    {
        _shootInfo = new ShootInfo();
        _shootInfo.pos = position.ToV3();
        _shootInfo.dir = direction.ToV3();
    }
}
