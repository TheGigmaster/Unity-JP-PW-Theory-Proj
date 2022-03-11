using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[DefaultExecutionOrder(1000)]
public class AppBehavior : MonoBehaviour
{
    public static AppBehavior Instance;
    public GameObject playerBox;

    private EnumGameState gameState;

    public EnumGameState GameState
    {
        get { return gameState; }
        //set
        //{
        //    if (Enum.IsDefined(typeof(EnumGameState), value))
        //    {
        //        gameState = value;
        //    }
        //    else
        //    {
        //        Debug.Log($"ERROR: {value} is not a valid game state!");
        //    }

        //}
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        playerBox = GameObject.FindGameObjectWithTag("PlayerBox");
    }

    public void StartGame()
    {
        Debug.Log("Game start stuff would happen now");
        gameState = EnumGameState.Running;
        // Other stuff that goes in when you start the game
    }
    public void ExitGame()
    {
        //SaveProfile();
        Debug.Log("Stuff that would happen when application exits would happen now");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
    Application.Quit();
#endif
    }

    public enum EnumGameState
    {
        MainMenu = 0,
        Running = 1
    }
}
