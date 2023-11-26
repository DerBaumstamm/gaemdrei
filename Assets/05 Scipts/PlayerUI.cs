using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
    
    [SerializeField] private TextMeshProUGUI playerName;
    private NetworkVariable<FixedString128Bytes> networkPlayerName = new("Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> health = new(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag.Equals("bullet"))
        {
            health.Value -= 1;
            Debug.Log(health.Value);
        }
    }

    public override void OnNetworkSpawn()
    {
        PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerName.text = playerData.playerName.ToString();
    }
}
