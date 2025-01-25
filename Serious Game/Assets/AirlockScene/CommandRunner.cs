using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CommandRunner : MonoBehaviour
{
    public static CommandRunner Instance;
    public Transform commandGrid;       // The grid where commands are placed
    public RobotController robot;       // Reference to the robot
    public Color highlightColor = Color.yellow; // Color to highlight the current command

    public GameObject prefab;
    private List<CommandData> savedCommands = new List<CommandData>();
    public bool gameCompleted = false;

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

    private int previousChildCount;

    void Update()
    {
        if (commandGrid)
        {
            if (commandGrid.childCount != previousChildCount)
            {
                previousChildCount = commandGrid.childCount;
                SaveCommands(); // Save when a child is added or removed
            }
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
        if (scene.name == "AirlockScene")
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
            LoadCommands();
            if (gameCompleted == true)
            {
                OnGameCompleted();
            }
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
                for (int i = 0; i < block.repeatCount; i++) // Repeat the command
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



    public void SaveCommands()
    {
        savedCommands.Clear();
        foreach (Transform child in commandGrid)
        {
            CommandBlock block = child.GetComponent<CommandBlock>();
            if (block != null)
            {
                savedCommands.Add(new CommandData(block.commandType, block.repeatCount));
            }
        }
    }

    public void LoadCommands()
    {
        foreach (Transform child in commandGrid)
        {
            if (child.name != "Divider")
            {
                Destroy(child.gameObject);
            }
        }

        foreach (CommandData commandData in savedCommands)
        {
            if (prefab != null)
            {
                GameObject newcom = Instantiate(prefab, commandGrid);
                CommandBlock block = newcom.GetComponent<CommandBlock>();
                if (block != null)
                {
                    block.commandType = commandData.commandType;
                    block.repeatCount = commandData.repeatCount;

                    // Update dropdown value if using TMP Dropdown
                    TMP_Dropdown dropdown = newcom.GetComponentInChildren<TMP_Dropdown>();
                    if (dropdown != null)
                    {
                        dropdown.value = commandData.repeatCount - 1; // Assuming dropdown options are 1-based
                    }
                }
            }
        }
    }

    public void OnGameCompleted()
    {
        gameCompleted = true;
        GameObject uiObject = GameObject.Find("Completed");
        if (uiObject != null)
        {
            //set the children as active
            foreach (Transform child in uiObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        Debug.Log("Game Completed!");
    }

}
