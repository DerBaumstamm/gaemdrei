using TMPro;
using UnityEngine;

public class GameOverPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject winnerGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private TextMeshPro playerNameText;

    private void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += gameMultiplayer_PlayerDataNetworkList_OnListChanged;       
        updatePlayer();
    }


    private void gameMultiplayer_PlayerDataNetworkList_OnListChanged(object sender, System.EventArgs e)
    {
        updatePlayer();
    }

    private void updatePlayer()
    {
        if (GameMultiplayer.Instance.isPlayerIndexConnected(playerIndex))
        {
            show();
            PlayerData playerData = GameMultiplayer.Instance.getPlayerDataFromPlayerIndex(playerIndex);
            winnerGameObject.SetActive(playerData.clientId == GameMultiplayer.Instance.getWinnerId());
            playerNameText.text = playerData.playerName.ToString();
            playerVisual.setPlayerColor(GameMultiplayer.Instance.getPlayerColor(playerData.colorId));
            playerVisual.setPlayerMaterial(GameMultiplayer.Instance.getPlayerMaterial(playerData.colorId));
        }
        else
        {
            hide();
        }
    }
    private void show()
    {
        gameObject.SetActive(true);
    }

    private void hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= gameMultiplayer_PlayerDataNetworkList_OnListChanged;
    }
}
