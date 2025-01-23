using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FakeTerminal : MonoBehaviour
{
    public static FakeTerminal Instance;
   // public EscapeToMain escapeToMain;
    public TMP_InputField commandInput;      // Reference to the TMP Input Field for command input
    public TextMeshProUGUI outputText;       // Reference to the output text area
    public ScrollRect scrollRect;            // Reference to the ScrollRect component for scrolling
    private bool terminalActive = true;      // Flag to track if the terminal is active
    private bool gameStarted = false;         // Flag to track if the game has started

    private string inputPrefix = ">>> ";     // Prefix for the input line

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        commandInput.onValueChanged.AddListener(UpdateInputField);
        commandInput.onSubmit.AddListener(HandleCommand);
        commandInput.text = inputPrefix;     // Initialize the input field with the prefix
        commandInput.caretPosition = inputPrefix.Length; // Set caret position after the prefix

        //Welcome to game, available commands: \nstart, settings, quit
        outputText.text = "Welcome to the Ordinary Hero BCS.1.1 Game Terminal!\nType 'start' to begin the game...\nType 'help' for more information\n";
        ScrollToBottom(); // Ensure the terminal starts scrolled down
    }

    // Update to check for escape for closing
    void Update()
    {
        // Check if the Escape key is pressed
        if (terminalActive && Input.GetKeyDown(KeyCode.Escape))
        {
            HideTerminal();
        }
    }

    private void UpdateInputField(string text)
    {
        // Ensure the prefix cannot be removed
        if (!text.StartsWith(inputPrefix))
        {
            commandInput.text = inputPrefix;
            commandInput.caretPosition = inputPrefix.Length;
        }
        else if (commandInput.caretPosition < inputPrefix.Length)
        {
            // Prevent caret from moving into the prefix
            commandInput.caretPosition = inputPrefix.Length;
        }
    }

    private void HandleCommand(string input)
    {
        string command = input.Substring(inputPrefix.Length).Trim(); // Extract user input after the prefix

        if (!string.IsNullOrEmpty(command))
        {
            ProcessCommand(command);
        }

        // Reset the input field to the prefix and reactivate it
        commandInput.text = inputPrefix;
        commandInput.caretPosition = inputPrefix.Length;
        commandInput.ActivateInputField();
    }

    private void ProcessCommand(string command)
    {
        if (command.ToLower() == "start")
        {
            if (gameStarted)
            {
                outputText.text += $"\n> {command}\nGame has already started.\n";
                return;
            }
            else
            {
                gameStarted = true;
                StartCoroutine(StartGame());
            }   
        }
        else if (command.ToLower() == "quit")
        {
            outputText.text += $"\n> {command}\nQuitting the game...\n";
            //TODO: Add here a way to quit the game
        }
        else if (command.ToLower() == "settings")
        {
            outputText.text += $"\n> {command}\nRedirecting to settings page...\n";
            //TODO: Add here link to settings page
        }
        else if (command.ToLower() == "help")
        {
            outputText.text += $"\n> {command}\nOrdinary Hero is a serious game meant for persuading students to study Computer Science & Engineering bachelor at TU/e\n Type 'settings' to change the settings,\n'quit' to end the game,\nor 'start' to start the game\n";
        }
        else
        {
            outputText.text += $"\n> {command}\nCommand not recognized. Try 'start'.\n";
        }

        // Force scroll to the bottom after adding new text
        ScrollToBottom();
    }

    private IEnumerator StartGame()
    {
        outputText.text += "Mission Day 118\n";
        ScrollToBottom();
        yield return new WaitForSeconds(3);

        outputText.text += "Logging in to your account...\n";
        ScrollToBottom();
        yield return new WaitForSeconds(3);

        //fade out background image of terminal
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);


        outputText.text += "Mission Operations Specialist\n";
        ScrollToBottom();
        yield return new WaitForSeconds(1);

        outputText.text += "ID number: 108099\n";
        ScrollToBottom();
        yield return new WaitForSeconds(1);

        outputText.text += "TU/e Computer Science graduate 2025\n";
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases(); // Ensure all UI changes are processed
        scrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom
    }
    
    public void HideTerminal()
    {
        // Deactivate the terminal GameObject when leaving the scene
        scrollRect.gameObject.SetActive(false);
        terminalActive = false;
    }

    public void ShowTerminal()
    {
        // Reactivate the terminal GameObject when returning to the terminal scene
        scrollRect.gameObject.SetActive(true);
        terminalActive = true;
    }
    
}
