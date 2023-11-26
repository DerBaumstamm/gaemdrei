using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    //public event EventHandler OnStateChanged;
    //public event EventHandler OnLocalGamePaused;
    //public event EventHandler OnLocalGameUnpaused;
    //public event EventHandler OnMultiplayerGamePaused;
    //public event EventHandler OnMultiplayerGameUnpaused;
    //public event EventHandler OnLocalPlayerReadyChanged;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    [SerializeField] private Transform playerPrefab;

    //private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    //private bool isLocalPlayerReady;
    //private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    //private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    //private float gamePlayingTimerMax = 90f;
    //private bool isLocalGamePaused = false;
    //private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    //private Dictionary<ulong, bool> playerReadyDictionary;
    //private Dictionary<ulong, bool> playerPausedDictionary;
    //private bool autoTestGamePausedState;

    private void Awake()
    {
        Instance = this;

        //playerReadyDictionary = new Dictionary<ulong, bool>();
        //playerPausedDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        //state.OnValueChanged += State_OnValueChanged;
        //isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }
    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        //autoTestGamePausedState = true;
    }
}
