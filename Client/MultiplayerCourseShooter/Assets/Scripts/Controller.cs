using GameDevWare.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun[] _guns;
    [SerializeField] private float _mouseSpeedX = 2f;
    [SerializeField] private float _mouseSpeedY = 2f;
    [SerializeField] private float _respawnDelay = 3f;

    private bool _hold;
    private PlayerGun _currGun;
    private int _currGunIndex;

    public bool IsPrinting { get; set; }

    private void Start()
    {
        ActivateGun(0);
    }

    void Update()
    {
        if (_hold) return;
        if (IsPrinting) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool sitdown = Input.GetKey(KeyCode.LeftControl);

        bool isShoot = Input.GetMouseButton(0);

        bool nextWeapon = Input.GetKeyDown(KeyCode.E);
        bool prevWeapon = Input.GetKeyDown(KeyCode.Q);

        _player.SetInput(h, v, mouseX * _mouseSpeedX);
        _player.RotateHead(-mouseY * _mouseSpeedY);
        _player.RotateBody();
        if (jump)
            _player.Jump();

        SendMove();

        if (sitdown != _player.Sitdown)
        {
            _player.Sitdown = sitdown;
            _player.SitDown();
            SitdownInfo info = new SitdownInfo();
            info.sit = sitdown;
            SendSitdown(info);
        }

        if (nextWeapon)
            ActivateGun(++_currGunIndex);
        else if(prevWeapon)
            ActivateGun(--_currGunIndex);

        if (isShoot && _currGun.TryShoot())
            SendShoot(_currGun.ShootInfo);
    }

    private void ActivateGun(int index)
    {
        index = CycleGunIndex(index);
        _currGunIndex = index;
        _currGun = _guns[_currGunIndex];

        for (int i = 0; i < _guns.Length; i++)
        {
            if (i == index)
                _guns[i].gameObject.SetActive(true);
            else
                _guns[i].gameObject.SetActive(false);
        }

        SendCurrentGun();
    }

    private int CycleGunIndex(int index)
    {
        index = index < 0 ? _guns.Length - Mathf.Abs(index) : index % _guns.Length;
        return index;
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

    public void SendSitdown(SitdownInfo info)
    {
        info.key = MultiplayerManager.Instance.GetSessionID();
        string json = JsonUtility.ToJson(info);
        MultiplayerManager.Instance.SendMessage("sit", json);
    }

    public void Respawn(string jsonRespawnInfo)
    {
        RespawnInfo info = JsonUtility.FromJson<RespawnInfo>(jsonRespawnInfo);
        StartCoroutine(Hold());
        _player.transform.position = new Vector3(info.x, 0f, info.z);
        _player.SetInput(0, 0, 0);
        _player.GetRotateInfo(out float rotateX, out float rotateY);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX",  info.x},
            {"pY", 0},
            {"pZ", info.z},
            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},
            {"rX", rotateX},
            {"rY", rotateY},
        };
        MultiplayerManager.Instance.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSecondsRealtime(_respawnDelay);
        _hold = false;
    }

    private void SendCurrentGun()
    {
        string key = MultiplayerManager.Instance.GetSessionID();
        ChangeGunInfo info = new ChangeGunInfo(key, _currGunIndex);

        string json = JsonUtility.ToJson(info);
        MultiplayerManager.Instance.SendMessage("gun", json);
    }
}
