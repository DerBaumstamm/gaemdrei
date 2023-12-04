using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer bodyMeshRenderer;
    [SerializeField] private MeshRenderer topBottomMeshRenderer;
    private Material bodyMaterial;
    private Material topBottomMaterial;

    private void Awake()
    {
        bodyMaterial = new Material(bodyMeshRenderer.material);
        topBottomMaterial = new Material(topBottomMeshRenderer.material);
        bodyMeshRenderer.material = bodyMaterial;
        topBottomMeshRenderer.material = topBottomMaterial;
    }

    public void setPlayerColor(Color color)
    {
        topBottomMaterial.color = color;
    }
    public void setPlayerMaterial(Material mat)
    {
        bodyMaterial.mainTexture = mat.mainTexture;
    }
}
