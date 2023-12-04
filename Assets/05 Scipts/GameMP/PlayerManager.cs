using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private GameObject playerPistol;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //when player spawns it sets score to 0 and applies previously selected skin
    public override void OnNetworkSpawn()
    {
        GameMultiplayer.Instance.setPlayerScore(0);
        PlayerData playerData = GameMultiplayer.Instance.getPlayerDataFromClientId(OwnerClientId);
        playerVisual.setPlayerColor(GameMultiplayer.Instance.getPlayerColor(playerData.colorId));
        playerVisual.setPlayerMaterial(GameMultiplayer.Instance.getPlayerMaterial(playerData.colorId));
    }
}
