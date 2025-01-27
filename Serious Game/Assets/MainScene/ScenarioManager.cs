using UnityEngine;
using System.Collections;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance;

    public bool gameStarted = false;
    private bool gateAOpened = false;
    private bool waitingForGate = false;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    //Start Scenario - played when player types 'start'
    public void OnStartCommand()
    {
        if (gameStarted)
        {
            TerminalUI.Instance.AddToTerminal("Game is already running!");
        }
        else
        {
            gameStarted = true;
            StartCoroutine(StartScenario());
        }
    }

    private IEnumerator StartScenario()
    {
        TerminalUI.Instance.AddToTerminal("Mission Day 118");
        yield return new WaitForSeconds(0.5f);

        TerminalUI.Instance.AddToTerminal("Logging in to your account...");
        yield return new WaitForSeconds(2);


        TerminalUI.Instance.AddToTerminal("Mission Operations Specialist");
        yield return new WaitForSeconds(0.5f);

        TerminalUI.Instance.AddToTerminal("ID number: 108099");
        yield return new WaitForSeconds(0.1f);

        TerminalUI.Instance.AddToTerminal("TU/e Computer Science graduate 2025");

        TerminalUI.Instance.AddToTerminal("Astronaut Status:");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Martinez = ONLINE");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Bottlíková = ONLINE");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Škařupa = ONLINE");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Gürkan = ONLINE");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Künü = ONLINE");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Leśniak = ONLINE");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Ronay = ONLINE");
        
        yield return new WaitForSeconds(0.5f);

        TerminalUI.Instance.AddToTerminal("Press ESC to hide Terminal");
        yield return StartCoroutine(WaitForTerminalToClose());
        
        yield return new WaitForSeconds(10f);
        
        TerminalUI.Instance.AddToTerminal("Martinez: Collected probe. Outside Lab-1.", "yellow");
        yield return new WaitForSeconds(1.5f);
        TerminalUI.Instance.AddToTerminal("Martinez: Open gate pls", "yellow");
        yield return new WaitForSeconds(1f);

        TerminalUI.Instance.AddToTerminal("open gate using command: open [gateId]");
        TerminalUI.Instance.AddToTerminal("Use the map to find the gateId");

        waitingForGate = true;
        // Start timed tip logic
        StartCoroutine(ShowTipsWhileWaitingForGate());
    }

    private IEnumerator SystemBreakdownScenario()
    {
        //Crackling sound, all cameras go to static, except for the satellite view.
        //Alarm sounds start playing. Warning pops up on main screen “ALL SYSTEMS OFFLINE” 

        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("CRITICAL SYSTEM ERROR", "red");
        yield return new WaitForSeconds(0.5f);
        //TerminalUI.Instance.AddToTerminal("TYPE 'reboot' for SYSTEM RESTART", "red");
        TerminalUI.Instance.AddToTerminal("BREACH DETECTED", "red");
        yield return new WaitForSeconds(0.5f);
        TerminalUI.Instance.AddToTerminal("POWER NETWORK OFFLINE", "red");
        yield return new WaitForSeconds(2f);

        TerminalUI.Instance.AddToTerminal("Martinez: What was that?", "yellow");
        //TerminalUI.Instance.AddToTerminal("Martinez: there has been an explosion", "yellow");

        yield return new WaitForSeconds(4f);

        TerminalUI.Instance.AddToTerminal("CAPCOM:  There was an explosion in Lab-2 during the repair work, and we’ve lost the entire module", "blue");

        yield return new WaitForSeconds(5f);

        TerminalUI.Instance.AddToTerminal("CAPCOM:  We’ve lost the rest of your team, we need to get you home safely", "blue");
        yield return new WaitForSeconds(2.6f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  Please return to airlock immediately", "blue");

        yield return new WaitForSeconds(5f);

        TerminalUI.Instance.AddToTerminal("Martinez:  OK, Airlock stuck", "yellow");

        yield return new WaitForSeconds(4f);

        TerminalUI.Instance.AddToTerminal("CAPCOM:  We’re deploying COSMO to assist", "blue");
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  Our engineers will control the robot to unlock", "blue");
        yield return new WaitForSeconds(2f);
        TerminalUI.Instance.AddToTerminal("Control the robot COSMO on your middle screen", "green");
    }

    // Called by CommandProcessor if user typed "open X"
    public void OnOpenGateCommand(string gateId)
    {
        if (!gameStarted)
        {
            TerminalUI.Instance.AddToTerminal("You need to start the game first. Type 'start'.");
            return;
        }

        if (!waitingForGate)
        {
            TerminalUI.Instance.AddToTerminal("No gate to open at this moment.");
            return;
        }

        // Check gate ID
        if (gateId == "A")
        {
            gateAOpened = true;
            waitingForGate = false;
            TerminalUI.Instance.AddToTerminal("Gate A is now open!");
            // Possibly do more logic after the gate is open...
        }
        else
        {
            TerminalUI.Instance.AddToTerminal($"Gate '{gateId}' not recognized. Check the map!");
        }
    }

    // Continuously shows tips if user hasn’t opened gate A yet
    private IEnumerator ShowTipsWhileWaitingForGate()
    {
        float elapsed = 0f;
        float firstTipTime = 5f;
        float secondTipTime = 10f;

        while (waitingForGate && !gateAOpened)
        {
            elapsed += Time.deltaTime;

            if (elapsed >= firstTipTime && firstTipTime > 0f)
            {
                TerminalUI.Instance.AddToTerminal("Tip1: You need 'open A'.");
                firstTipTime = -1; // Mark as used
            }
            if (elapsed >= secondTipTime && secondTipTime > 0f)
            {
                TerminalUI.Instance.AddToTerminal("Tip2: The correct command is literally 'open A'.");
                secondTipTime = -1;
            }

            yield return null; // wait a frame
        }

        // Once the gate is opened, we exit the loop
        // Continue with scenario...
        if (gateAOpened)
        {
            // Continue with scenario...
            yield return new WaitForSeconds(1f);
            TerminalUI.Instance.AddToTerminal("Martinez: thx", "yellow");

            yield return new WaitForSeconds(20f);

            StartCoroutine(SystemBreakdownScenario());
        }
    }

    // Continuously waits for terminal to close
    private IEnumerator WaitForTerminalToClose()
    {
        while (TerminalUI.Instance.terminalActive)
        {
            yield return null; // Wait for the next frame
        }
    }
}
