using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{
    //private Camera gameCamera;

    void Start()
    {
        //gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // For show; realized we can get it from the main property on the Camera class
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
    }

    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "PlayerBox") 
            {
                AppBehavior.Instance.StartGame();
                // Figured that MenuBehavior = behavior of just elements of the UI. Left game state management to app behavior.
            }
        }
    }

    public void OnExit()
    {
        AppBehavior.Instance.ExitGame();
    }


}
