using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private PlayerVisual playerVisual;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);       
    }

    //when player spawns it sets score to 0 and applies previously selected skin
    public override void OnNetworkSpawn()
    {
        GameMultiplayer.Instance.SetPlayerScore(0);
        PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(GameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
        playerVisual.SetPlayerMaterial(GameMultiplayer.Instance.GetPlayerMaterial(playerData.colorId));
    }
}
