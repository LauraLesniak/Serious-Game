using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class TerminalUI : MonoBehaviour
{
    public static TerminalUI Instance;        // Optional singleton for easy access

    [Header("UI References")]
    [SerializeField] private TMP_InputField commandInput;
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private TMP_Text terminalTwinText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Image bgImage;

    [Header("Behavior")]
    [SerializeField] private int maxLines = 10;
    [SerializeField] private string inputPrefix = ">>> ";
    private List<string> lines = new List<string>();
    public bool terminalActive = true;
    private bool debounceActive = false; // Debounce flag

    private void Awake()
    {
        // Basic singleton pattern
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Subscribe to input callbacks
        commandInput.onValueChanged.AddListener(OnInputChanged);
        commandInput.onSubmit.AddListener(OnCommandSubmitted);

        // Initialize prefix
        commandInput.text = inputPrefix;
        commandInput.caretPosition = inputPrefix.Length;

        // Example initial message
        AddToTerminal("Welcome to the Ordinary Hero BCS.1.1 Game Terminal!");
        AddToTerminal("Type 'start' to begin the game...");
        AddToTerminal("Type 'help' for more information");

        if (bgImage != null)
        {
            // Hide terminal on background click
            bgImage.GetComponent<Button>().onClick.AddListener(HideTerminal);
        }
    }

    // ----------------- Handling user input ----------------- //

    private void Update()
    {
        // Check if the Escape key is pressed
        if (terminalActive && Input.GetKeyDown(KeyCode.Escape))
        {
            debounceActive = false;
            HideTerminal();
        }
    }

    private void OnInputChanged(string newText)
    {
        // Prevent removing prefix
        if (!newText.StartsWith(inputPrefix))
        {
            commandInput.text = inputPrefix;
            commandInput.caretPosition = inputPrefix.Length;
        }
        else if (commandInput.caretPosition < inputPrefix.Length)
        {
            commandInput.caretPosition = inputPrefix.Length;
        }
    }

    private void OnCommandSubmitted(string fullInput)
    {
        // Extract just the command after prefix
        string command = fullInput.Substring(inputPrefix.Length).Trim();
        if (!string.IsNullOrEmpty(command))
        {
            // Hand off to the CommandProcessor
            CommandProcessor.Instance.ProcessCommand(command);
        }

        // Reset Input
        commandInput.text = inputPrefix;
        commandInput.caretPosition = inputPrefix.Length;
        commandInput.ActivateInputField();
    }

    // ----------------- Public Terminal Methods ----------------- //

    /// <summary>
    /// Add a new line of text to the terminal with an optional color.
    /// If colorNameOrHex is null or empty, no color is applied.
    /// </summary>
    // public void AddToTerminal(string newLine, string colorNameOrHex = null)
    // {
    //     // If a color is provided, wrap the text in <color=...> tags
    //     string finalLine = string.IsNullOrEmpty(colorNameOrHex)
    //         ? newLine
    //         : $"<color={colorNameOrHex}>{newLine}</color>";

    //     // Append new line to the output text
    //     outputText.text += $"{finalLine}\n";
    //     lines.Add(finalLine);

    //     // Truncate if we have too many lines
    //     while (lines.Count > maxLines)
    //     {
    //         lines.RemoveAt(0);
    //     }

    //     // Update the 3D twin text if needed
    //     if (terminalTwinText != null)
    //     {
    //         StringBuilder sb = new StringBuilder();
    //         foreach (var line in lines)
    //         {
    //             sb.AppendLine(line);
    //         }
    //         terminalTwinText.text = sb.ToString();
    //     }

    //     ScrollToBottom();
    // }
    public void AddToTerminal(string newLine, string colorNameOrHex = null)
    {
        // If a color is provided, wrap the text in <color=...> tags
        // (this color tag affects both the main terminal and the twin,
        // which might be your intended behavior)
        string finalLine = string.IsNullOrEmpty(colorNameOrHex)
            ? newLine
            : $"<color={colorNameOrHex}>{newLine}</color>";

        // 1) Append new line to the main terminal text normally
        outputText.text += $"{finalLine}\n";

        // 2) Keep track of the un-chunked line in our lines list
        lines.Add(finalLine);

        // 3) Truncate if we have too many lines stored
        while (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
        }

        // 4) Rebuild the TerminalTwin text -- *with chunking*
        RebuildTerminalTwinText();

        // 5) Scroll main terminal to bottom
        ScrollToBottom();
    }

    /// <summary>
    /// Rebuilds the terminalTwinText from our stored lines, 
    /// but each line is chunked at 39 characters so it doesn't overflow.
    /// </summary>
    private void RebuildTerminalTwinText()
    {
        if (terminalTwinText == null) return;

        const int maxCharsPerLine = 39;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // For each line in our 'lines' list, break it up into 39-char segments
        foreach (var line in lines)
        {
            int index = 0;
            while (index < line.Length)
            {
                int remaining = line.Length - index;
                int length = Mathf.Min(remaining, maxCharsPerLine);
                string chunk = line.Substring(index, length);
                sb.AppendLine(chunk);
                index += length;
            }

            // Handle case of an empty line
            if (line.Length == 0)
            {
                sb.AppendLine(""); // Just a blank line
            }
        }

        terminalTwinText.text = sb.ToString();
    }


    public void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ShowTerminal()
    {
        StartCoroutine(DebounceCoroutine()); // Start debounce coroutine
        terminalActive = true;
        scrollRect.gameObject.SetActive(true);
        bgImage.CrossFadeAlpha(1, 0.1f, false);
        StartCoroutine(ActivateInputFieldNextFrame());
        ScrollToBottom();
    }

    public void HideTerminal()
    {
        if (!ScenarioManager.Instance.gameStarted) return; // Prevent hiding terminal before the game starts
        if (debounceActive) return; // Prevent hiding if debounce is active
        scrollRect.gameObject.SetActive(false);
        terminalActive = false;
        bgImage.CrossFadeAlpha(0, 0.1f, false);
    }

    private IEnumerator ActivateInputFieldNextFrame()
    {
        yield return null;
        commandInput.Select();
        commandInput.ActivateInputField();
        commandInput.caretPosition = commandInput.text.Length;
    }

    private IEnumerator DebounceCoroutine()
    {
        debounceActive = true;
        yield return new WaitForSeconds(0.5f); // Adjust the debounce duration as needed
        debounceActive = false;
    }
}