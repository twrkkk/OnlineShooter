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

    private bool _isPrinting;
    private bool _showCursor;

    public bool IsPrinting
    {
        get
        {
            return _isPrinting;
        }
        set
        {
            _isPrinting = value;
            _showCursor = !value;
            if(_showCursor)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Start()
    {
        ActivateGun(0);
        _showCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None; 
        if(Input.GetMouseButtonDown(0)) 
            Cursor.lockState = CursorLockMode.Locked; 

        if (_hold) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (IsPrinting) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool sitdown = Input.GetKey(KeyCode.LeftControl);

        bool isShoot = Input.GetMouseButton(0);

        bool nextWeapon = Input.GetKeyDown(KeyCode.E);
        bool prevWeapon = Input.GetKeyDown(KeyCode.Q);

        _player.SetInput(h, v, mouseX * _mouseSpeedX);
        _player.RotateHead(-mouseY * _mouseSpeedY);
        //_player.RotateBody();
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

    public void Respawn(int index)
    {
        Debug.Log(index);
        MultiplayerManager.Instance._spawnPoints.GetPoint(index, out Vector3 position, out Vector3 rotation);

        StartCoroutine(Hold());
        _player.transform.position = position;
        _player.transform.eulerAngles = new Vector3(0f, rotation.y, 0f);
        _player.SetInput(0, 0, 0);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX",  position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},
            {"rX", 0},
            {"rY", rotation.y},
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
