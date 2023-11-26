using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Invoke(nameof(loadNewScene), 0.2f);     
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });      
    } 
    private void loadNewScene()
    {
        Loader.Load(Loader.Scene.LobbyMenu);
    }
}
