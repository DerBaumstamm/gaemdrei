using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreUi;
    [SerializeField] private Transform playerPrefab;

    private void Awake()    {
        Instance = this;
        scoreUi.text = GameMultiplayer.Instance.getLeaderboard();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += sceneManager_OnLoadEventCompleted;
        }
    }

    private void sceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {            
            if (SceneManager.GetActiveScene().name != Loader.Scene.GameOver.ToString())
            {
                Transform playerTransform = Instantiate(playerPrefab);
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            }
        }
    }

    public void updateLeaderboard()
    {
        scoreUi.text = GameMultiplayer.Instance.getLeaderboard();
    }
}
