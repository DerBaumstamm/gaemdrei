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
            LobbyMenu.Instance.leaveLobby();
            Loader.Load(Loader.Scene.MainMenu);
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            lobbyCreateUI.show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            LobbyMenu.Instance.quickJoin();
        });
        codeJoinButton.onClick.AddListener(() =>
        {
            LobbyMenu.Instance.joinWithCode(codeInputField.text);
        });
        lobbyTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        playerNameInputField.text = GameMultiplayer.Instance.getPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            GameMultiplayer.Instance.setPlayerName(newText);
        });
        LobbyMenu.Instance.OnLobbyListChanged += lobbyMenu_OnLobbyListChanged;
        updateLobbyList(new List<Lobby>());
    }

    private void lobbyMenu_OnLobbyListChanged(object sender, LobbyMenu.OnLobbyListChangedEventArgs e)
    {
        updateLobbyList(e.lobbyList);
    }

    private void updateLobbyList(List<Lobby> lobbyList)
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
            lobbyTransform.GetComponent<LobbyListSingleUI>().setLobby(lobby);
        }
    }
}
