using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSpeedX = 2f;
    [SerializeField] private float _mouseSpeedY = 2f;
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool sitdown = Input.GetKey(KeyCode.LeftControl);

        bool isShoot = Input.GetMouseButton(0);

        _player.SetInput(h, v, mouseX * _mouseSpeedX);
        _player.RotateHead(-mouseY * _mouseSpeedY);
        _player.RotateBody();
        if (jump)
            _player.Jump();

        SendMove();

        _player.Sitdown = sitdown;

        if (isShoot && _gun.TryShoot())
            SendShoot(_gun.ShootInfo);

    }

    public void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity);
        _player.GetRotateInfo(out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX},
            {"rY", rotateY},
        };
        MultiplayerManager.Instance.SendMessage("move", data);
    }

    public void SendShoot(ShootInfo shootInfo)
    {
        shootInfo.key = MultiplayerManager.Instance.GetSessionID();
        string json = JsonUtility.ToJson(shootInfo);

        MultiplayerManager.Instance.SendMessage("shoot", json);
    }

    public void SendSitdown()
    {

    }
}
