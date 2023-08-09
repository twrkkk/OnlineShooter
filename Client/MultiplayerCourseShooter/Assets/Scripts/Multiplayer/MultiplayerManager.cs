using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [field: SerializeField] public LossCounter LossCounter { get; private set; }
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;
    private ChatLogic _chatLogic;
    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }

    private async void Connect()
    {
        var data = new Dictionary<string, object>()
        {
            { "speed", _player.Speed},
            { "hp", _player.maxHealth},
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

        _room.OnStateChange += OnChange;

        _room.OnMessage<string>("enShoot", MakeEnemyShoot);
        _room.OnMessage<string>("enSit", EnemySitdown);
        _room.OnMessage<string>("mes", NewChatMessage);
    }

    private void NewChatMessage(string message)
    {
        _chatLogic.ShowNewMessage(message);
    }

    private void EnemySitdown(string value)
    {
        SitdownInfo info = JsonUtility.FromJson<SitdownInfo>(value);

        if (!_enemies.ContainsKey(info.key))
        {
            Debug.LogError("There is not enemy, but he tries to sitdown");
            return;
        }

        _enemies[info.key].Sit(in info);
    }

    private void MakeEnemyShoot(string jsonShootInfo)
    {
        ShootInfo info = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (!_enemies.ContainsKey(info.key))
        {
            Debug.LogError("There is not enemy, but he tries to shoot");
            return;
        }

        _enemies[info.key].Shoot(info);
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (!isFirstState) return;

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId)
                CreatePlayer(player); // it's my player
            else
                CreateEnemy(key, player);

        });

        state.players.OnAdd += CreateEnemy;
        state.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        var pl = Instantiate(_player, position, Quaternion.identity);

        pl.TryGetComponent<Controller>(out Controller controller);
        if (controller != null)
            _room.OnMessage<string>("respawn", controller.Respawn);

        pl.TryGetComponent<ChatLogic>(out ChatLogic chatLogic);
        if(chatLogic != null)   
            _chatLogic = chatLogic;

        player.OnChange += pl.onChange;
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);

        _enemies.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player value)
    {
        if (!_enemies.ContainsKey(key)) return;

        var enemy = _enemies[key];
        enemy.Destroy();
        _enemies.Remove(key);
    }

    private void Disconnect()
    {
        _room.Leave();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Disconnect();
    }

    public new void SendMessage(string key, object data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetSessionID() => _room.SessionId;
}
