using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameOverPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject winnerGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private TextMeshPro playerNameText;

    private void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameMultiplayer_PlayerDataNetworkList_OnListChanged;       
        UpdatePlayer();
    }


    private void GameMultiplayer_PlayerDataNetworkList_OnListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (GameMultiplayer.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            winnerGameObject.SetActive(playerData.clientId == GameMultiplayer.Instance.GetWinnerId());
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
