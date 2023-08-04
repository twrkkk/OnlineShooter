using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string Shoot = "Shoot";
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;
    private void OnEnable() =>
        _gun.onShoot += ShootAnimation;

    private void OnDisable() =>
        _gun.onShoot -= ShootAnimation;

    private void ShootAnimation()
    {
        _animator.SetTrigger(Shoot);
    }
}
