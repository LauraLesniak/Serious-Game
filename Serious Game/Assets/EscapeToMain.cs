using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EscapeToMain : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Hide the terminal before switching scenes
            FakeTerminal.Instance.HideTerminal();

            // Load the MainScene
            SceneManager.LoadScene("MainScene");
        }

    }
}
