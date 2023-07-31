using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _mouseSpeedX = 2f;
    [SerializeField] private float _mouseSpeedY = 2f;
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool jump = Input.GetKeyDown(KeyCode.Space);

        _player.SetInput(h, v, mouseX * _mouseSpeedX);
        _player.RotateHead(-mouseY * _mouseSpeedY);
        _player.RotateBody();
        if (jump)
            _player.Jump();

        SendMove();
    }

    public void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
        };
        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
