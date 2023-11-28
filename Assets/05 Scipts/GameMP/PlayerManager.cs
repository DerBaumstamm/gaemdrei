using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private TMP_Text scoreUi;
    private float updateIntervall;
    private float updatesPerSecond = 1;

    private void Awake()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        GameMultiplayer.Instance.SetPlayerScore(0);
        PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(GameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
        playerVisual.SetPlayerMaterial(GameMultiplayer.Instance.GetPlayerMaterial(playerData.colorId));
    }
    public void updatePlayerCounter()
    {
        if (!IsOwner) return;

        if (Time.time >= updateIntervall)
        {
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            scoreUi.text = playerData.score.ToString();
            updateIntervall = Time.time + 1f / updatesPerSecond;
        }
        
    }
}
