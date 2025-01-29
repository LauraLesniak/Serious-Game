using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    public static CommandProcessor Instance; // Optional singleton

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void ProcessCommand(string command)
    {
        // For convenience, unify case
        string cmd = command.ToLower();

        // Echo in terminal first
        TerminalUI.Instance.AddToTerminal("");
        TerminalUI.Instance.AddToTerminal($"> {command}");

        // BASIC: parse command
        if (cmd == "start")
        {
            // Possibly call into a GameManager or Scenario
            ScenarioManager.Instance.OnStartCommand();
        }
        else if (cmd == "quit")
        {
            TerminalUI.Instance.AddToTerminal("Quitting the game...");
            // your quit logic
        }
        else if (cmd.StartsWith("open "))
        {
            string gateId = cmd.Substring(5).Trim().ToUpper();
            ScenarioManager.Instance.OnOpenGateCommand(gateId);
        }
        else if (cmd.StartsWith("send "))
        {
            string toSend = cmd.Substring(5).Trim();
            ScenarioManager.Instance.OnSendDiagramCommand(toSend);
        }
        else if (cmd == "help")
        {
            TerminalUI.Instance.AddToTerminal("Ordinary Hero is a serious game meant for persuading students to study Computer Science & Engineering bachelor at TU/e");
            // TerminalUI.Instance.AddToTerminal("type 'settings' to change the settings,");
            TerminalUI.Instance.AddToTerminal("type 'quit' to end the game,");
            TerminalUI.Instance.AddToTerminal("type 'start' to start the game");
        }
        else
        {
            TerminalUI.Instance.AddToTerminal("Unrecognized command.");
        }
    }
}
