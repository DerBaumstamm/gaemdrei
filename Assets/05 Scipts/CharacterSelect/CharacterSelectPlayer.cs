using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{

    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private Button kickButton;
    [SerializeField] private TextMeshPro playerNameText;

    private void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            LobbyMenu.Instance.KickPlayer(playerData.playerId.ToString());
            GameMultiplayer.Instance.kickPlayer(playerData.clientId);
        });

    }
    private void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameMultiplayer_PlayerDataNetworkList_OnListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;
        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void GameMultiplayer_PlayerDataNetworkList_OnListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if(GameMultiplayer.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            playerNameText.text = playerData.playerName.ToString();
            playerVisual.SetPlayerColor(GameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
            playerVisual.SetPlayerMaterial(GameMultiplayer.Instance.GetPlayerMaterial(playerData.colorId));
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayer_PlayerDataNetworkList_OnListChanged;
    }
}
