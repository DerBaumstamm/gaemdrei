using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer bodyMeshRenderer;
    private Material material;

    private void Awake()
    {
        material = new Material(bodyMeshRenderer.material);
        bodyMeshRenderer.material = material;
    }

    public void SetPlayerColor(Color color)
    {
        material.color = color;
    }
    public void SetPlayerMaterial(Material mat)
    {
        material.mainTexture = mat.mainTexture;
    }
}
