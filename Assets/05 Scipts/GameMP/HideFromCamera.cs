
using UnityEngine;

public class HideFromCamera : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToHide;
    private void OnPreCull()
    {
        foreach (var obj in objectsToHide)
        {
            obj.SetActive(false);
        }
    }
    private void OnPostRender()
    {
        foreach (var obj in objectsToHide)
        {
            obj.SetActive(true);
        }
    }
}
