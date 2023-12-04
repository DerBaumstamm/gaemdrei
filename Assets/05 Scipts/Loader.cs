using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        LoadingScene,
        MainMenu,
        LobbyMenu,
        CharacterSelect,
        GameMP,
        GameOver,
    }
   
    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;        
        SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void loadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void loaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
