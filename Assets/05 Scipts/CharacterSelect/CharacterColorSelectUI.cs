using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectUI : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.changePlayerColor(colorId);
        });
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += gameMultiplayer_OnPlayerDataNetworkListChanged;
        image.color = GameMultiplayer.Instance.getPlayerColor(colorId);
        updateIsSelected();
    }

    private void gameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        updateIsSelected();
    }

    private void updateIsSelected()
    {
        if(GameMultiplayer.Instance.getPlayerData().colorId == colorId)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= gameMultiplayer_OnPlayerDataNetworkListChanged;
    }
}
