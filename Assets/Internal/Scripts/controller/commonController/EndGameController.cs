using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : NetworkBehaviour
{
    public NetworkVariable<int> endGame = new NetworkVariable<int>(0);

    public static EndGameController instance;
    private List<ulong> clientId = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].TryGetComponent<NetworkObject>(out var networkObject))
                {

                    clientId.Add(networkObject.OwnerClientId);
                }
            }
        }
    }

    public GameObject endGameContainer;
    private void Start()
    {
        endGameContainer.SetActive(false);
    }
    public void EndGameOut()
    {
        try
        {
            if (IsServer)
            {
                NetworkManager.Singleton.Shutdown();
                if (SceneController.instance != null)
                {
                    SceneController.instance.ChangeScene(SceneController.SceneName.Lobby, true);
                }
            }
            else
            {
                NetworkManager.Singleton.DisconnectClient(OwnerClientId);
                if (SceneController.instance != null)
                {
                    SceneController.instance.ChangeScene(SceneController.SceneName.Lobby, true);
                }
            }
        }
        catch (Exception e)
        {
            if (SceneController.instance != null)
            {
                SceneController.instance.ChangeScene(SceneController.SceneName.Lobby, true);
            }
        }
    }
    private void Update()
    {
        if (endGame.Value == 1)
        {
            endGameContainer.SetActive(true);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc()
    {
        endGame.Value = 1;
    }
}
