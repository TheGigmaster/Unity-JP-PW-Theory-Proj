using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// AppBehavior handles game states, including resetting to the start menu and camera position based on game state (since it won't follow anything).
// Would also handle save/load/persist if this app bothered with any of that.
public class AppBehavior : MonoBehaviour
{

    public static AppBehavior Instance;

    // Don't think I actually need this for AppBehavior?
    //public GameObject playerBox;

    // I like to do game states as an enum, as opposed to a bunch of bools or individual scenes (where possible).
    [SerializeField]
    private EnumGameState gameState;

    // Getter for gameState. I want other things to happen based on gameState of the instanced AppManager. 
    // Made a setter too for practice, but since AppManager should be the only thing that sets gameState I don't reeeally need it.
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
        // I don't need to switch scenes with the way the game is structured. These checks/DontDestroy are more as a reminder to myself for future stuff.
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //playerBox = GameObject.FindGameObjectWithTag("PlayerBox");
        gameState = EnumGameState.MainMenu;

        // Loadprofile stuff would go here
    }

    public void StartGame()
    {
        Debug.Log("Game start stuff would happen now");
        gameState = EnumGameState.Running;
        // Other stuff that goes in when you start the game, mainly box things. MenuBehavior should already be listening for the current game state and react accordingly.
    }
    public void ExitGame()
    {
        gameState = EnumGameState.Exiting;
        // SaveProfile type stuff would go here
        Debug.Log("Stuff that would happen when application exits would happen now");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
    Application.Quit();
#endif
    }

    public void GotoMainMenu()
    {
        gameState = EnumGameState.MainMenu;
        GameBehavior.Instance.GameReset();
        // Other stuff that needs to get reset when we go to menu, mainly the box position and rigidbody.gravity stuff
    }

    public enum EnumGameState
    {
        MainMenu = 1,
        Running = 2,
        Exiting = 3
    }
}
