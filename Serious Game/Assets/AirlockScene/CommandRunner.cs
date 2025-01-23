using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommandRunner : MonoBehaviour
{
    public static CommandRunner Instance;
    public Transform commandGrid;       // The grid where commands are placed
    public RobotController robot;       // Reference to the robot
    public Color highlightColor = Color.yellow; // Color to highlight the current command

    //make sure game is saved on load
    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject uiObject = GameObject.Find("Commands");
        if (uiObject != null)
        {
            commandGrid = uiObject.transform;
        }
        GameObject runButton = GameObject.Find("RunButton");
        if (runButton != null)
        {
            Button button = runButton.GetComponent<Button>();
            button.onClick.AddListener(RunCommands);
        }
    }

    public void RunCommands()
    {
        StartCoroutine(ExecuteCommands());
    }

    private IEnumerator ExecuteCommands()
    {
        foreach (Transform commandBlock in commandGrid)
        {
            CommandBlock block = commandBlock.GetComponent<CommandBlock>();
            Image blockImage = commandBlock.GetComponent<Image>();
            Color originalColor = blockImage.color;

            if (block != null)
            {

                // Highlight the current command
                blockImage.color = highlightColor;

                switch (block.commandType)
                {
                    case CommandType.MoveForward:
                        yield return StartCoroutine(robot.MoveForward());
                        break;

                    case CommandType.TurnLeft:
                        yield return StartCoroutine(robot.TurnLeft());
                        break;

                    case CommandType.TurnRight:
                        yield return StartCoroutine(robot.TurnRight());
                        break;
                }
                // Revert the color back to the original
                blockImage.color = originalColor;
            }   
        }
    }
}
