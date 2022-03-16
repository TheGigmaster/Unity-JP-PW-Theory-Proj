using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject playerBox;
    private GameObject newBox;

    public GameObject dogBoxPrefab;
    public GameObject chickenBoxPrefab;
    public GameObject catBoxPrefab;
    public GameObject defaultBoxPrefab;

    public Color newBoxColor;
    public PhysicMaterial newBoxPM;
    public string newBoxType;

    public GameObject PlayerBox { get 
        {
            Debug.Log($"Someone is asking for player box, returning named {playerBox.name}");
            return playerBox; 
        } 
    }

    public static GameBehavior Instance;

    private void Awake()
    {
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
        playerBox = GameObject.FindGameObjectWithTag("PlayerBox");
    }

    public void SpawnBox()
    {
        //playerBox.SendMessage("Despawn");
        playerBox.GetComponent<IBox>().Despawn();
        switch(newBoxType)
        {
            case "catButton":
                newBox = catBoxPrefab;
                break;

            case "dogButton":
                newBox = dogBoxPrefab;
                break;

            case "chickenButton":
                newBox = chickenBoxPrefab;
                break;

            default:
                newBox = defaultBoxPrefab;
                break;
        } // end switch

        Vector3 newPosition = new Vector3(newBox.transform.position.x, 35.0f, newBox.transform.position.z);
        playerBox = Instantiate(newBox, newPosition, newBox.transform.rotation);

        playerBox.GetComponent<Rigidbody>().isKinematic = false;
        playerBox.GetComponent<IBox>().SetAttributes(newBoxPM, newBoxColor);
    }

    public void GameReset()
    {
        Destroy(playerBox);
        playerBox = Instantiate(defaultBoxPrefab);
        playerBox.GetComponent<Rigidbody>().isKinematic = true;
    }

}
