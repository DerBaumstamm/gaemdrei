using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Transform playerPrefab;

    void Start()
    {
        text.text = GameMultiplayer.Instance.GetWinnerName() + " won the Game!";
        Cursor.lockState = CursorLockMode.None;
    }

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {            
            LobbyMenu.Instance.LeaveLobby();            
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
}
