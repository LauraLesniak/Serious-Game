using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FakeTerminal : MonoBehaviour
{
    [Header("UI References")]
    public static FakeTerminal Instance;
    public TMP_InputField commandInput;      // Reference to the TMP Input Field for command input
    public TextMeshProUGUI outputText;       // Reference to the output text area
    public TMP_Text terminalTwinText; // Reference to the TerminalTwin TextMeshPro component
    public ScrollRect scrollRect;            // Reference to the ScrollRect component for scrolling
    public Image bgImage;

    [Header("Terminal Behavior")]
    public int maxLines = 5; 
    public bool terminalActive = true;      // Flag to track if the terminal is active
    private bool gameStarted = false;         // Flag to track if the game has started
    private bool debounceActive = false; // Debounce flag
    private string inputPrefix = ">>> ";     // Prefix for the input line

    private AudioSource audioSource;

    private List<string> lines = new List<string>();
    
    // ---------------- Gate-related state ----------------
    private bool waitingForOpenGateCommand = false; // Are we expecting the user to type "open A"?
    private bool gateAOpened = false;               // Has gate A been opened already?


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (terminalTwinText != null)
            {
                DontDestroyOnLoad(terminalTwinText.gameObject);
            }
        } else {
            Destroy(gameObject);
            if (terminalTwinText != null)
            {
                Destroy(terminalTwinText.gameObject);
            }
        }
    }

    private void Start()
    {

        commandInput.onValueChanged.AddListener(UpdateInputField);
        commandInput.onSubmit.AddListener(HandleCommand);
        commandInput.text = inputPrefix;     // Initialize the input field with the prefix
        commandInput.caretPosition = inputPrefix.Length; // Set caret position after the prefix

        //Welcome to game, available commands: \nstart, settings, quit
        AddToTerminal("Welcome to the Ordinary Hero BCS.1.1 Game Terminal!");
        AddToTerminal("Type 'start' to begin the game...");
        AddToTerminal("Type 'help' for more information");
        //outputText.text = "Welcome to the Ordinary Hero BCS.1.1 Game Terminal!\nType 'start' to begin the game...\nType 'help' for more information\n";

        if (bgImage != null)
        {
            bgImage.GetComponent<Button>().onClick.AddListener(HideTerminal);
        }
        
        UpdateTerminalTwinText(); // Update TerminalTwin text initially
    }

    // Update to check for escape for closing
    void Update()
    {
        // Check if the Escape key is pressed
        if (terminalActive && Input.GetKeyDown(KeyCode.Escape))
        {
            debounceActive = false;
            HideTerminal();
        }
    }

    // -------------------- Core Terminal Methods --------------------- //

    private bool isPlayingBeep = false; 
    private void PlayBeepSound()
    {
        if (audioSource != null && !isPlayingBeep)
        {
            StartCoroutine(PlayBeepSoundCoroutine());
        }
    }

    private IEnumerator PlayBeepSoundCoroutine()
    {
        isPlayingBeep = true;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length); // Wait for the beep sound to finish
        isPlayingBeep = false;
    }

    /// <summary>
    /// Add a new line of text to the terminal. Automatically truncates old lines
    /// if we exceed maxLines, then updates both outputText and terminalTwinText.
    /// </summary>
    public void AddToTerminal(string newLine)
    {

        //play beep sound
        PlayBeepSound();

        outputText.text += $"{newLine}\n";

        // Make sure we always treat it as a separate line
        lines.Add(newLine);

        // If we exceed maxLines, remove from the front (oldest line)
        while (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
        }

        // Rebuild the full text and apply it to both terminal displays
        UpdateTerminalTwinText();
        //RebuildTerminalOutput();
        ScrollToBottom();
    }

    private void UpdateTerminalTwinText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lines.Count; i++)
        {
            sb.AppendLine(lines[i]);
        }

        // Apply to the 3D terminal
        if (terminalTwinText != null)
        {
            terminalTwinText.text = sb.ToString();
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
        AddToTerminal("");
        AddToTerminal($"> {command}");

        // ---------------- NEW: Check if we are waiting for the "open gate" command ----------------
        if (waitingForOpenGateCommand && !gateAOpened)
        {
            // Example: parse "open A"
            if (command.ToLower().StartsWith("open "))
            {
                string gateId = command.Substring(5).Trim().ToUpper(); // everything after 'open '
                if (gateId == "A")
                {
                    gateAOpened = true;
                    AddToTerminal("Gate A is now open! Good job.");
                    // Once gate is opened, we can do any follow-up logic here.
                    return;
                }
                else if (gateId == "B")
                {
                    AddToTerminal("This gate cannot be opened");
                }
                else
                {
                    AddToTerminal($"Gate '{gateId}' not recognized. Check the map for the correct ID.");
                    return;
                }
            }
            // If we get here, the user typed something else while we want "open a"
            AddToTerminal("Command not recognized. Use 'open [gateId]', e.g. 'open A'.");
            return;
        }

        // ---------------- Existing commands ----------------
        if (command.ToLower() == "start")
        {
            if (gameStarted)
            {
                AddToTerminal("Game has already started.");
                //outputText.text += $"\n> {command}\nGame has already started.\n";
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
            AddToTerminal("Quitting the game...");
            //TODO: Add here a way to quit the game
        }
        else if (command.ToLower() == "settings")
        {
            AddToTerminal("Redirecting to settings page...");
            //TODO: Add here link to settings page
        }
        else if (command.ToLower() == "help")
        {
            AddToTerminal("Ordinary Hero is a serious game meant for persuading students to study ");
            AddToTerminal("Computer Science & Engineering bachelor at TU/e");
            AddToTerminal("Type 'settings' to change the settings,");
            AddToTerminal("'quit' to end the game,");
            AddToTerminal("or 'start' to start the game");
            //outputText.text += $"\n> {command}\nOrdinary Hero is a serious game meant for persuading students to study Computer Science & Engineering bachelor at TU/e\n Type 'settings' to change the settings,\n'quit' to end the game,\nor 'start' to start the game\n";
        }
        else
        {
            AddToTerminal("Command not recognized. Try 'start'.");
            //outputText.text += $"\n> {command}\nCommand not recognized. Try 'start'.\n";
        }

        // Force scroll to the bottom after adding new text
        ScrollToBottom();
    }

    private IEnumerator StartGame()
    {
        AddToTerminal("Mission Day 118");
        ScrollToBottom();
        yield return new WaitForSeconds(0.5f);

        AddToTerminal("Logging in to your account...");
        ScrollToBottom();
        yield return new WaitForSeconds(2);

        //fade out background image of terminal
        //gameObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);

        AddToTerminal("Mission Operations Specialist");
        ScrollToBottom();
        yield return new WaitForSeconds(0.5f);

        AddToTerminal("ID number: 108099");
        ScrollToBottom();
        yield return new WaitForSeconds(0.1f);

        AddToTerminal("TU/e Computer Science graduate 2025");
        ScrollToBottom();

        AddToTerminal("Astronaut Status:");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Martinez = ONLINE");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Bottlíková = ONLINE");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Škařupa = ONLINE");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Gürkan = ONLINE");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Künü = ONLINE");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Leśniak = ONLINE");
        yield return new WaitForSeconds(0.1f);
        AddToTerminal("Ronay = ONLINE");
        
        yield return new WaitForSeconds(0.5f);

        AddToTerminal("Press ESC to hide Terminal");

        // wait for closing of terminal
        yield return StartCoroutine(WaitForTerminalToClose());
        yield return new WaitForSeconds(10f);
        AddToTerminal("Martinez: Collected probe. Outside Lab-1.");
        yield return new WaitForSeconds(1.5f);
        AddToTerminal("Martinez: Open gate pls");

        yield return new WaitForSeconds(2f);
        AddToTerminal("open gate using command: open [gateId]");
        AddToTerminal("Find the correct gate near Lab-1 on the map");

        // ---------------- NEW: Start waiting for "open A" command with tips ----------------
        yield return StartCoroutine(WaitForOpenGate());
        
        // After the gate is opened, continue your game logic...
        AddToTerminal("You have successfully opened Gate A. The mission continues!");
    }

    private IEnumerator WaitForOpenGate()
    {
        waitingForOpenGateCommand = true;

        float elapsedTime = 0f;
        float firstTipTime = 10f;   // time in seconds to show first tip
        float secondTipTime = 30f; // time in seconds to show second tip

        // Wait until gateAOpened becomes true (typed correctly)
        while (!gateAOpened)
        {
            elapsedTime += Time.deltaTime;

            // Show first tip if enough time has passed (and we haven't shown it yet)
            if (elapsedTime >= firstTipTime && firstTipTime > 0f)
            {
                AddToTerminal("Tip: The correct syntax is 'open [gateId]'. For example, 'open A'.");
                firstTipTime = -1f; // mark tip shown so we don't show it again
            }

            // Show second tip a bit later if still not opened
            if (elapsedTime >= secondTipTime && secondTipTime > 0f)
            {
                AddToTerminal("Tip: The gate ID is 'A'. Type 'open A'.");
                secondTipTime = -1f;
            }

            yield return null;
        }

        // Gate is opened, stop waiting
        waitingForOpenGateCommand = false;
    }

    private IEnumerator WaitForTerminalToClose()
    {
        while (terminalActive)
        {
            yield return null; // Wait for the next frame
        }
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases(); // Ensure all UI changes are processed
        scrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom
    }




    // -------------------- Show / Hide Terminal --------------------- //
    
    public void HideTerminal()
    {
        if (!gameStarted) return; // Prevent hiding terminal before the game starts
        if (debounceActive) return; // Prevent hiding if debounce is active
        // Deactivate the terminal GameObject when leaving the scene
        scrollRect.gameObject.SetActive(false);
        terminalActive = false;
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, 0.1f, false);

        UpdateTerminalTwinText(); // Update TerminalTwin text when hiding terminal
    }

    private IEnumerator ActivateInputFieldAfterFrame()
    {
        yield return null; // Wait for one frame
        commandInput.Select();
        commandInput.ActivateInputField();
        commandInput.caretPosition = commandInput.text.Length; // Set caret position after the prefix
    }

    public void ShowTerminal()
    {
        StartCoroutine(DebounceCoroutine()); // Start debounce coroutine
        
        // Reactivate the terminal GameObject when returning to the terminal scene
        scrollRect.gameObject.SetActive(true);
        terminalActive = true;
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, 0.1f, false);
        //commandInput.Select();
        StartCoroutine(ActivateInputFieldAfterFrame());
        //commandInput.ActivateInputField();
    }

    private IEnumerator DebounceCoroutine()
    {
        debounceActive = true;
        yield return new WaitForSeconds(0.5f); // Adjust the debounce duration as needed
        debounceActive = false;
    }
    
}
