using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreUi;
    [SerializeField] private Transform playerPrefab;

    //public event EventHandler OnStateChanged;
    //public event EventHandler OnLocalGamePaused;
    //public event EventHandler OnLocalGameUnpaused;
    //public event EventHandler OnMultiplayerGamePaused;
    //public event EventHandler OnMultiplayerGameUnpaused;
    //public event EventHandler OnLocalPlayerReadyChanged;

    //private enum State
    //{
    //   WaitingToStart,
    //    CountdownToStart,
    //    GamePlaying,
    //    GameOver,
    //}

    

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
        scoreUi.text = GameMultiplayer.Instance.GetLeaderboard();
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
    public void updateLeaderboard()
    {
        scoreUi.text = GameMultiplayer.Instance.GetLeaderboard();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        //autoTestGamePausedState = true;
    }
}
