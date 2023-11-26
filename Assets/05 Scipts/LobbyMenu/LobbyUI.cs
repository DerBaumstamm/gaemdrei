using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using System.ComponentModel;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button codeJoinButton;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;


    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            LobbyMenu.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenu);
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            lobbyCreateUI.Show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            LobbyMenu.Instance.QuickJoin();
        });
        codeJoinButton.onClick.AddListener(() =>
        {
            LobbyMenu.Instance.JoinWithCode(codeInputField.text);
        });
        lobbyTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        playerNameInputField.text = GameMultiplayer.Instance.GetPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            GameMultiplayer.Instance.SetPlayerName(newText);
        });
        LobbyMenu.Instance.OnLobbyListChanged += LobbyMenu_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void LobbyMenu_OnLobbyListChanged(object sender, LobbyMenu.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }
}
