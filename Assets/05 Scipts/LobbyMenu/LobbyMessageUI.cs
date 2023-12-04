using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;


    private void Awake()
    {
        closeButton.onClick.AddListener(hide);
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame += gameMultiplayer_OnFailedToJoinGame;
        LobbyMenu.Instance.OnCreateLobbyStarted += gameLobby_OnCreateLobbyStarted;
        LobbyMenu.Instance.OnCreateLobbyFailed += gameLobby_OnCreateLobbyFailed;
        LobbyMenu.Instance.OnJoinStarted += gameLobby_OnJoinStarted;
        LobbyMenu.Instance.OnJoinFailed += gameLobby_OnJoinFailed;
        LobbyMenu.Instance.OnQuickJoinFailed += gameLobby_OnQuickJoinFailed;
        closeButton.gameObject.SetActive(false);
        hide();
    }

    private void gameLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        showMessage("Could not find a Lobby to Quick Join!");
        closeButton.gameObject.SetActive(true);
    }

    private void gameLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        showMessage("Failed to join Lobby!");
        closeButton.gameObject.SetActive(true);
    }

    private void gameLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        showMessage("Joining Lobby...");
    }

    private void gameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        showMessage("Failed to create Lobby!");
        closeButton.gameObject.SetActive(true);
    }

    private void gameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        showMessage("Creating Lobby...");
    }

    private void gameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            showMessage("Failed to connect");
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            showMessage(NetworkManager.Singleton.DisconnectReason);
            closeButton.gameObject.SetActive(true);
        }
    }

    private void showMessage(string message)
    {
        show();
        messageText.text = message;
    }

    private void show()
    {
        gameObject.SetActive(true);
    }

    private void hide()
    {
        gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame -= gameMultiplayer_OnFailedToJoinGame;
        LobbyMenu.Instance.OnCreateLobbyStarted -= gameLobby_OnCreateLobbyStarted;
        LobbyMenu.Instance.OnCreateLobbyFailed -= gameLobby_OnCreateLobbyFailed;
        LobbyMenu.Instance.OnJoinStarted -= gameLobby_OnJoinStarted;
        LobbyMenu.Instance.OnJoinFailed -= gameLobby_OnJoinFailed;
        LobbyMenu.Instance.OnQuickJoinFailed -= gameLobby_OnQuickJoinFailed;
    }

}