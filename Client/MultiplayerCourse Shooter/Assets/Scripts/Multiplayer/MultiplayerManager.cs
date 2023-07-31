using Colyseus;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private EnemyController _enemy;
    private ColyseusRoom<State> _room;
    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }

    private async void Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>("state_handler");

        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (!isFirstState) return;

        state.players.ForEach((key, player) => {
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
        Instantiate(_player, position, Quaternion.identity);
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        player.OnChange += enemy.OnChange;
    }

    private void RemoveEnemy(string key, Player value)
    {

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
}
