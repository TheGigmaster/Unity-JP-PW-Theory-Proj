using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// MenuBehavior determines behavior of UI elements. 
// The comments from the MenuUIHandler class script in a prior demo mentioned that delaying execution for UI class(es?) before all other default scripts is useful,
// so that everything else can happen before the UI has to base what it shows off of those things. Sso.... monkey see, monkey do.
[DefaultExecutionOrder(1000)]
public class MenuBehavior : MonoBehaviour
{
    // Instanced for the lulz. Probably don't need this honestly.
    public MenuBehavior Instance;

    // The two 'markers' in scene our camera will travel between based on menu/game state.
    // I like this approach because you can slap cameras on them in the editor and adjust on the fly to fine-tune how things will look, without having to add code
    // or futz with the main camera.
    private GameObject mainMenuPostion;
    private GameObject gamePosition;

    // The speed at which our menu transitions will happen.
    [SerializeField]
    private float menuSpeed = 0.15f;

    private bool isCameraMoving = false;

    // Stuff we'll grab in our start block
    private GameObject mainMenuContainer;
    private GameObject gameMenuContainer;
    private Animation mainMenuAnimator;
    private CanvasGroup mainMenuCG;
    private Animation gameMenuAnimator;
    private CanvasGroup gameMenuCG;

    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Slider bounceSlider;

    // Need to put in stuff here for holding player selections during game play

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

    void Start()
    {
        // I want our main menu position to be the starting position of the camera in the editor window
        mainMenuPostion = GameObject.Find("MainMenuCamMarker");

        // I have an object with a camera I used to get a good 'feel' for where the gameplay camera would be. 
        gamePosition = GameObject.Find("GameCamMarker");

        // We'll selectively turn these on and off according to the AppBehavior.gameState of our AppManager
        mainMenuContainer = transform.Find("IntroMenuContainer").gameObject;
        gameMenuContainer = transform.Find("GameMenuContainer").gameObject;

        // We use these components to make menu transitions look nice, and to use CanvasGroup.interactable as a boolean switch below
        mainMenuAnimator = mainMenuContainer.GetComponent<Animation>();
        mainMenuCG = mainMenuContainer.GetComponent<CanvasGroup>();
        gameMenuAnimator = gameMenuContainer.GetComponent<Animation>();
        gameMenuCG = gameMenuContainer.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        // Game is started by the player left clicking on the on-screen box.
        if (AppBehavior.Instance.GameState == AppBehavior.EnumGameState.MainMenu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleLeftClick();
            } // end if 
        }

        // MenuBehavior listens for game state from AppBehavior object, as long as we don't have the moveCamera coroutine currently running,
        // Then changes menus when states change. Switch method below.
        if (!isCameraMoving) 
        {
            EvalGameState(AppBehavior.Instance.GameState);
        }
        
    }

    // This switch checks for the game state and updates active menu elements accordingly.
    // One of the fun details here is because AppBehavior.Instance.GameState will NEVER be null (it will
    // at least be GameState = 0, or Main Menu, we don't need to check null!
    // The clips in the animation objects on both canvas group containers handle things like alpha, Y-coord and interactability.
    private void EvalGameState(AppBehavior.EnumGameState _gameState)
    { // Go, go, gadget abstract
        switch (_gameState.ToString())
        {
            case "Running":
                if (gameMenuCG.interactable == false)
                {
                    mainMenuAnimator.Play("MainMenuOff");
                    gameMenuAnimator.Play("GameMenuOn");
                    StartCoroutine(MoveCamera(Camera.main.gameObject, gamePosition, menuSpeed));
                } // endif
                break;

            case "MainMenu":
                if (mainMenuCG.interactable == false)
                {
                    gameMenuAnimator.Play("GameMenuOff");
                    mainMenuAnimator.Play("MainMenuOn");
                    StartCoroutine(MoveCamera(Camera.main.gameObject, mainMenuPostion, menuSpeed));
                } // endif
                break;

            case "Exiting":
                mainMenuContainer.SetActive(false);
                gameMenuContainer.SetActive(false);
                break;
        } // end switch
    }

    private IEnumerator MoveCamera(GameObject camera, GameObject marker, float speed)
    {
        // Puts EvalGameState on pause so we don't waste time and instantiate uneeded coroutines
        isCameraMoving = true;

        // MarkerPos will be where we're going; the refVelocity is needed for Vector3.SmoothDamp.
        Vector3 markerPos = marker.transform.position;
        Vector3 refVelocity = Vector3.zero;

        // The API doc for Vector3.SmoothDamp says that smoothtime (speed here) is 'Approximately the time it will take to reach the target'.
        // This generally seems to be an overestimate by a factor of 2, from what I saw when testing, and makes a timer unusable for a do/while
        // terminator. Calculating the current distance every update from the running coroutine shouldn't be a big ask, and is infinitely more 
        // reliable.
        float currDistance;

        do
        {
            camera.transform.position = (Vector3.SmoothDamp(camera.transform.position, markerPos, ref refVelocity, speed));            

            currDistance = Vector3.Distance(camera.transform.position, markerPos);

            yield return null;
        } // end do
        // If we don't do ' > 0.01f' here, the engine will keep on pulling our camera to an ?E^-BigNumber-nth distance from the marker.
        while (currDistance > 0.01f);

        // Make sure we end up where we were planning to.
        camera.transform.position = marker.transform.position;

        isCameraMoving = false;
    }

    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "PlayerBox") 
            {
                OnGame();
                // Figured that MenuBehavior = behavior of just elements of the UI. Left game state management to app behavior.
            } // end if
        } // end if
    }

    // It's game behavior's job to know what object is the player box at any given time. Then we need the Box script object on it so we can
    // use it's method for changing color.
    public void OnChangeBoxColor()
    {
        //var playerBox = GameBehavior.Instance.PlayerBox.GetComponent<Box>();
        float r = redSlider.value;
        float g = greenSlider.value;
        float b = blueSlider.value;

        GameBehavior.Instance.newBoxColor = new Color(r, g, b);
    }

    public void OnChangeBoxBounce()
    {
        PhysicMaterial pm = new PhysicMaterial();
        pm.bounciness = bounceSlider.value;
        pm.bounceCombine = PhysicMaterialCombine.Maximum;
        GameBehavior.Instance.newBoxPM = pm;
    }
    // These are here mainly so menu elements can get them. Enter/save/exit is the purview of the app manager, so we just get methods from it.
    public void OnGame()
    {
        AppBehavior.Instance.StartGame();
    }

    public void OnMenu()
    {
        AppBehavior.Instance.GotoMainMenu();
    }


    public void OnExit()
    {
        AppBehavior.Instance.ExitGame();
    }

    public void OnSpawn()
    {
        GameBehavior.Instance.SpawnBox();
    }

    public void OnOpen()
    {
        GameBehavior.Instance.PlayerBox.GetComponent<IBox>().OpenBox();
    }

    private void SetType(string type)
    {
        GameBehavior.Instance.newBoxType = type;
        Debug.Log($"Menu manager told game manager that next box type is { type }");
    }


}
