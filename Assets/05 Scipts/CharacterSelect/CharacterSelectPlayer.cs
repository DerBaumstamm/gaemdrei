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
            PlayerData playerData = GameMultiplayer.Instance.getPlayerDataFromPlayerIndex(playerIndex);
            LobbyMenu.Instance.kickPlayer(playerData.playerId.ToString());
            GameMultiplayer.Instance.kickPlayer(playerData.clientId);
        });

    }
    private void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += gameMultiplayer_PlayerDataNetworkList_OnListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += characterSelectReady_OnReadyChanged;
        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        updatePlayer();
    }

    private void characterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        updatePlayer();
    }

    private void gameMultiplayer_PlayerDataNetworkList_OnListChanged(object sender, System.EventArgs e)
    {
        updatePlayer();
    }

    private void updatePlayer()
    {
        if(GameMultiplayer.Instance.isPlayerIndexConnected(playerIndex))
        {
            show();
            PlayerData playerData = GameMultiplayer.Instance.getPlayerDataFromPlayerIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.isPlayerReady(playerData.clientId));
            playerNameText.text = playerData.playerName.ToString();
            playerVisual.setPlayerColor(GameMultiplayer.Instance.getPlayerColor(playerData.colorId));
            playerVisual.setPlayerMaterial(GameMultiplayer.Instance.getPlayerMaterial(playerData.colorId));
        }
        else
        {
            hide();
        }
    }
    private void show()
    {
        gameObject.SetActive(true);
    }

    private void hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= gameMultiplayer_PlayerDataNetworkList_OnListChanged;
    }
}
