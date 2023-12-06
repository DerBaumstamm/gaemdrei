using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscapeUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        hide();
    }

    private void Awake()
    {      
        mainMenuButton.onClick.AddListener(() =>
        {
            LobbyMenu.Instance.leaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
        continueButton.onClick.AddListener(() =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            hide();
        });
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
}
