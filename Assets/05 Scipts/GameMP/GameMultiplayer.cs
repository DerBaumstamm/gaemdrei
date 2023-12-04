using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMultiplayer : NetworkBehaviour
{
    public const int MAX_PLAYER_AMOUNT = 4;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public static GameMultiplayer Instance { get; private set; }
    public static bool playMultiplayer = true;

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;

    [SerializeField] private List<Color> playerColorList;
    [SerializeField] private List<Material> playerMaterialList;
    private NetworkList<PlayerData> playerDataNetworkList;

    private string playerName;
    private string winnerName;
    private ulong winnerId;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 999));
        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += playerDataNetworkList_OnListChanged;
    }

    public string getPlayerName()
    {
        return playerName;
    }

    public void setPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }

    private void playerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void startHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += networkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += networkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += networkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();    
    }

    private void networkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                // Disconnected!
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void networkManager_OnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
            colorId = getFirstUnusedColorId(),
        });
        setPlayerNameServerRpc(getPlayerName());
        setPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    private void networkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {            
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelect.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }        
        connectionApprovalResponse.Approved = true;
    }

    public void startClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += networkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += networkManager_Client_OnClientConnectedCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void networkManager_Client_OnClientConnectedCallback(ulong clientId)
    {
        setPlayerNameServerRpc(getPlayerName());
        setPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void setPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.playerName = playerName;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void setPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.playerId = playerId;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    private void networkManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    public bool isPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public int getPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }

    public PlayerData getPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                return playerData;
            }
        }
        return default;
    }

    public PlayerData getPlayerData()
    {
        return getPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public PlayerData getPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }

    public Color getPlayerColor(int colorId)
    {
        return playerColorList[colorId];
    }
    public Material getPlayerMaterial(int colorId)
    {
        return playerMaterialList[colorId];
    }

    public void changePlayerColor(int colorId)
    {
        changePlayerColorServerRpc(colorId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void changePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
    {
        if (!isColorAvailable(colorId))
        {
            // Color not available
            return;
        }

        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.colorId = colorId;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    private bool isColorAvailable(int colorId)
    {
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            if (playerData.colorId == colorId)
            {
                // Already in use
                return false;
            }
        }
        return true;
    }

    private int getFirstUnusedColorId()
    {
        for (int i = 0; i < playerColorList.Count; i++)
        {
            if (isColorAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }

    public void setPlayerScore(int score)
    {
        setPlayerScoreServerRpc(score);
    }

    [ServerRpc(RequireOwnership = false)]
    private void setPlayerScoreServerRpc(int score, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.score = score;
        playerDataNetworkList[playerDataIndex] = playerData;
    }
    public void addPlayerScore(ulong clientId)
    {
        addPlayerScoreServerRpc(clientId);
        foreach(PlayerData playerData in playerDataNetworkList)
        {
            if(playerData.score >= 10)
            {
                winnerName = playerData.playerName.ToString();
                winnerId = playerData.clientId;
                Loader.loadNetwork(Loader.Scene.GameOver);
                return;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void addPlayerScoreServerRpc(ulong clientId)
    {
        Debug.Log(clientId + " | AddPlayerScoreRPC in GameMultiplayer");
        int playerDataIndex = getPlayerDataIndexFromClientId(clientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.score += 1;
        playerDataNetworkList[playerDataIndex] = playerData;
        
    }

    public string getLeaderboard()
    {
        string leaderboard = "";
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            leaderboard += "<color=#" + ColorUtility.ToHtmlStringRGBA(getPlayerColor(playerData.colorId)) + "> " + playerData.score + " | " + playerData.playerName.ToString() + "</color>\n";
        }
        return leaderboard;
    }

    public void kickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        networkManager_Server_OnClientDisconnectCallback(clientId);
    }

    public string getWinnerName()
    {
        return winnerName;
    }
    public ulong getWinnerId()
    {
        return winnerId;
    }
}
