using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{


    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (GameMultiplayer.Instance != null)
        {
            Destroy(GameMultiplayer.Instance.gameObject);
        }

        if (LobbyMenu.Instance != null)
        {
            Destroy(LobbyMenu.Instance.gameObject);
        }

        if(PlayerManager.Instance != null)
        {
            Destroy(PlayerManager.Instance.gameObject);
        }
    }
}