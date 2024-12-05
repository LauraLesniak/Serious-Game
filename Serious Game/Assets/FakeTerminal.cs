using UnityEngine;
using TMPro;

public class FakeTerminal : MonoBehaviour
{
    public TMP_InputField commandInput;      // Reference to the TMP Input Field for command input
    public TextMeshProUGUI outputText;       // Reference to the output text area

    private void Start()
    {
        commandInput.onSubmit.AddListener(HandleCommand);
        outputText.text = "Welcome to the Game Terminal!\nType 'start' to begin the game...\n";
    }

    private void HandleCommand(string command)
    {
        if (!string.IsNullOrEmpty(command))
        {
            ProcessCommand(command);
            commandInput.text = string.Empty;
            commandInput.ActivateInputField();
        }
    }

    private void ProcessCommand(string command)
    {
        if (command.ToLower() == "start")
        {
            outputText.text += "\n> " + command + "\nStarting the game...\n";
            StartGame();
        }
        else
        {
            outputText.text += "\n> " + command + "\nCommand not recognized. Try 'start'.\n";
        }

        // Scroll to bottom after adding new text
        Canvas.ForceUpdateCanvases();
        // If needed, add logic here to ensure scrolling stays at the bottom
    }

    private void StartGame()
    {
        // Logic to transition to the game scene
        Debug.Log("Game Started!");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("YourGameSceneName");
    }
}
