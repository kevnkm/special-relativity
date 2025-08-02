using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public InputActionProperty leftActivate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (leftActivate.action.WasPressedThisFrame())
        {
            Debug.Log("ButtonPressed");
            ToNextScene();
        }
    }

    public void ToNextScene()
    {

        Debug.Log("ButtonPressed");
        SceneManager.LoadScene("MainScene");
    }
}
