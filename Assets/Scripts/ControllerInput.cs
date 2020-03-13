using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject panel;
    public bool isPaused = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Gamepad.current.startButton.wasPressedThisFrame)
        if (Input.GetButtonDown("Start"))
        {
            Debug.Log("start button pressed");
            isPaused = !isPaused;
            panel.SetActive(isPaused);
        }
    }
}
