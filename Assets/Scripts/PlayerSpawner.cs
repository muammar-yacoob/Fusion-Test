using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    private NetworkObject playerObject;

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerObject = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            Runner.Despawn(playerObject);
        }
    }
}