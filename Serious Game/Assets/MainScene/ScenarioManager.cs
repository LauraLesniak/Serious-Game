using UnityEngine;
using System.Collections;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance;

    public bool gameStarted = false;
    private bool gateAOpened = false;
    private bool waitingForGate = false;
    private bool waitingForDiagram = false;
    private bool waitingForFlow = false;
    private bool waitingForList = false;

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

    private void Start()
    {
        RobotController.GameCompleted += OnAirlockGameCompleted;
    }

    private void OnDestroy()
    {
        RobotController.GameCompleted -= OnAirlockGameCompleted;
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
            
            //debug skipping progress
            //StartCoroutine(CircuitCompletedScenario());
        }
    }

    //1 START SCENARIO

    private IEnumerator StartScenario()
    {

        CameraManager.Instance.SetState(0);
        MainScreen.Instance.SetState(0);

        TerminalUI.Instance.AddToTerminal("Mission Day 118");
        yield return new WaitForSeconds(0.5f);

        TerminalUI.Instance.AddToTerminal("Logging in to your account...");
        yield return new WaitForSeconds(2);


        SoundManager.Instance.Play("running-beeps");
        TerminalUI.Instance.AddToTerminal("Mission Operations Specialist");
        yield return new WaitForSeconds(0.5f);

        TerminalUI.Instance.AddToTerminal("ID number: 108099");
        yield return new WaitForSeconds(0.1f);

        TerminalUI.Instance.AddToTerminal("TU/e Computer Science graduate 2025");
        yield return new WaitForSeconds(0.1f);
        TerminalUI.Instance.AddToTerminal("Astronaut Status:");
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
        
        MainScreen.Instance.SetState(1);
        
        TerminalUI.Instance.AddToTerminal("Martinez: Collected probe. Outside Lab-1.", "yellow");
        yield return new WaitForSeconds(1.5f);
        TerminalUI.Instance.AddToTerminal("Martinez: Open gate pls", "yellow");
        yield return new WaitForSeconds(1f);

        TerminalUI.Instance.AddToTerminal("open gate using command: open [gateId]");
        TerminalUI.Instance.AddToTerminal("Use the map to find the gateId");

        waitingForGate = true;
        // Start timed tip logic
        StartCoroutine(ShowTipsWhileWaitingForGate()); //NEXT SCENARIO
    }

    //2 GATE OPENED

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
            yield return new WaitForSeconds(5f);
            TerminalUI.Instance.AddToTerminal("Martinez: I'm outside", "yellow");
            yield return new WaitForSeconds(15f);

            StartCoroutine(SystemBreakdownScenario()); //NEXT SCENARIO
        }
    }

    //3 SYSTEM BREAKDOWN

    private IEnumerator SystemBreakdownScenario()
    {
        SoundManager.Instance.Play("pip");
        MainScreen.Instance.SetState(2);
        CameraManager.Instance.SetState(1);
        yield return new WaitForSeconds(2f);

        //Crackling sound, all cameras go to static, except for the satellite view.
        //Alarm sounds start playing. Warning pops up on main screen “ALL SYSTEMS OFFLINE” 

        SoundManager.Instance.Play("error");
        TerminalUI.Instance.AddToTerminal("State Report: Severe breach in Lab-2", "red");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("CRITICAL SYSTEM ERROR", "red");
        SoundManager.Instance.Play("alarm");
        yield return new WaitForSeconds(0.5f);
        //TerminalUI.Instance.AddToTerminal("TYPE 'reboot' for SYSTEM RESTART", "red");
        TerminalUI.Instance.AddToTerminal("BREACH DETECTED", "red");
        yield return new WaitForSeconds(0.5f);
        TerminalUI.Instance.AddToTerminal("POWER NETWORK OFFLINE", "red");
        yield return new WaitForSeconds(2f);

        FullscreenExplosion.Instance.PlayExplosion();
        yield return new WaitForSeconds(2f);

        TerminalUI.Instance.AddToTerminal("Martinez: What was that?", "yellow");
        //TerminalUI.Instance.AddToTerminal("Martinez: there has been an explosion", "yellow");

        yield return new WaitForSeconds(4f);
        CameraManager.Instance.SetState(2);
        SoundManager.Instance.Play("voiceover1");
        TerminalUI.Instance.AddToTerminal("CAPCOM: Martinez, this is CAPCOM. Do you copy?", "#24c1ff");
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: I have some difficult news to share. There was an explosion in Lab-2 during the repair work, and we’ve lost the entire module.", "#24c1ff");
        yield return new WaitForSeconds(6.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: I’m deeply sorry to report that we’ve also lost the rest of your team.", "#24c1ff");
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: We understand the gravity of this situation, and our priority now is to get you home safely.", "#24c1ff");
        yield return new WaitForSeconds(4f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: To do that, we’ll need your help restoring power to critical systems.", "#24c1ff");
        yield return new WaitForSeconds(3.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: Stand by—we’re preparing step-by-step instructions to rewire the generator and will send them to you shortly", "#24c1ff");
        yield return new WaitForSeconds(5.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: Hang in there, Martinez. We’re here with you", "#24c1ff");

        yield return new WaitForSeconds(4f);

        TerminalUI.Instance.AddToTerminal("Martinez:  OK, Airlock stuck", "yellow");

        yield return new WaitForSeconds(2f);
        SoundManager.Instance.Play("voiceover2");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: Copy that—you’re stuck in the airlock. It looks like the explosion triggered the airlock to seal itself from the inside", "#24c1ff");
        yield return new WaitForSeconds(5.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: We’re deploying COSMO to assist. He’s powered independently, so he should be able to reach you and override the lock.", "#24c1ff");
        yield return new WaitForSeconds(5.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: Sit tight—we’re working as quickly as we can to get you out of there. Keep us updated on your condition.”", "#24c1ff");
        yield return new WaitForSeconds(6f);

        TerminalUI.Instance.AddToTerminal("Control the robot COSMO on your middle screen", "green");
        
        //set main screen to airlock game and wait until game is finished
        CameraManager.Instance.SetState(3);
        MainScreen.Instance.SetState(3);
    }

    //4 AIRLOCK GAME COMPLETED

    private void OnAirlockGameCompleted()
    {
        StartCoroutine(AirlockOpenedScenario());
    }

    //should be ran after airlock game is completed
    private IEnumerator AirlockOpenedScenario()
    {
        SoundManager.Instance.Play("correct");
        TerminalUI.Instance.AddToTerminal("Airlock game completed!", "green");
        MainScreen.Instance.SetState(4);
        yield return new WaitForSeconds(3f);
        
        TerminalUI.Instance.AddToTerminal("Martinez: Alright, awaiting instructions.", "yellow");
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.Play("beep");
        TerminalUI.Instance.AddToTerminal("<color=#ff33f8>Flight Director:</color> <color=green>@MissionOperationsSpecialist</color> <color=#ff33f8> transmit the power restoration schematics to Martinez without delay.</color>");
        yield return new WaitForSeconds(3.5f);
        TerminalUI.Instance.AddToTerminal("Open the wiring app and solve the problem!", "green");
        MainScreen.Instance.SetState(5);

        //at this point, hide the airlock game to prevent it from showing up in other games
        //find a gameobject with name 'AirlockGame' and set it inactive
        GameObject airlockGame = GameObject.Find("AirlockGame");
        if (airlockGame != null)
        {
            airlockGame.SetActive(false);
        }
    }

    //5 CIRCUIT GAME COMPLETED

    public void onCircuitGameCompleted()
    {
        StartCoroutine(CircuitCompletedScenario());
    }

    private IEnumerator CircuitCompletedScenario()
    {
        SoundManager.Instance.Play("correct");
        TerminalUI.Instance.AddToTerminal("Circuit game completed!", "green");
        MainScreen.Instance.SetState(4); //set screen back to power outage
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("Martinez: Send me the diagram!", "yellow");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("type 'send diagram.pdf' to send diagram to Martinez");

        waitingForDiagram = true;
    }

    //5 DIAGRAM SENT

    public void onDiagramSent()
    {
        StartCoroutine(DiagramSentScenario());
    }

    private IEnumerator DiagramSentScenario()
    {
        //Reaction to diagram sent
        yield return new WaitForSeconds(2f);
        TerminalUI.Instance.AddToTerminal("Martinez: Recieved. On it.", "yellow");
        yield return new WaitForSeconds(5f);

        //lights back on on cameras set middle screen to show power back on
        TerminalUI.Instance.AddToTerminal("POWER SYSTEM RECOVERED", "green");
        MainScreen.Instance.SetState(6); //set screen back to power on
        CameraManager.Instance.SetState(4);

        //CAPCOM talks about next mission
        MainScreen.Instance.SetState(7); //show not enough oxygen
        yield return new WaitForSeconds(2.5f);
        SoundManager.Instance.Play("voiceover3");
        yield return new WaitForSeconds(1.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  Great work so far Martinez. We’re losing a significant amount of O2 through the breach, so here’s what we need you to do:", "#24c1ff");
        yield return new WaitForSeconds(5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  re-route the oxygen flow to prioritize the core, cargo, and escape pod modules.", "#24c1ff");
        yield return new WaitForSeconds(5.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  We’re preparing a schematic to help you maximize the flow, and we’ll send it over shortly.", "#24c1ff");
        yield return new WaitForSeconds(4f);

        //Flight director assigns next task
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.Play("beep");
        TerminalUI.Instance.AddToTerminal("<color=#ff33f8>Flight Director:</color> <color=green>@MissionOperationsSpecialist</color> <color=#ff33f8> send the oxygen optimization system details to Martinez now. We need to conserve as much as possible.</color>");
        yield return new WaitForSeconds(3.5f);
        TerminalUI.Instance.AddToTerminal("Open the pipe flow app and maximise flow!", "green");
        MainScreen.Instance.SetState(8); //show pipe flow game
    }

    //6 FLOW CONTROL COMPLETED

    public void onFlowControlCompleted()
    {
        StartCoroutine(FlowCompletedScenario());
    }

    //should be ran after airlock game is completed
    private IEnumerator FlowCompletedScenario()
    {
        SoundManager.Instance.Play("correct");
        TerminalUI.Instance.AddToTerminal("Flow game completed!", "green");
        MainScreen.Instance.SetState(7); //show not enough oxygen again
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("Martinez: Send me the flow schematic!", "yellow");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("type 'send flow.pdf' to send diagram to Martinez");

        waitingForFlow = true;
        
    }

    //7 FLOW SENT

    public void onFlowSent()
    {
        StartCoroutine(FlowSentScenario());
    }

    private IEnumerator FlowSentScenario()
    {
        //Reaction to flow sent
        yield return new WaitForSeconds(2f);
        TerminalUI.Instance.AddToTerminal("Martinez: Flow diagram recieved", "yellow");
        yield return new WaitForSeconds(5f);
        TerminalUI.Instance.AddToTerminal("Martinez: Done", "yellow");
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("Martinez: What's the reading? Success?", "yellow");
        yield return new WaitForSeconds(5f);

        //main screen shows success
        SoundManager.Instance.Play("correct");
        TerminalUI.Instance.AddToTerminal("State Report: O2 in core module optimal at 21%");
        MainScreen.Instance.SetState(9); //set screen to oxygen recovered

        //CAPCOM talks about next mission
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.Play("voiceover4");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  Great job, Martinez. Now, we need you to head to the cargo module", "#24c1ff");
        yield return new WaitForSeconds(3.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  You’ll need to gather a few essential items for your journey back.", "#24c1ff");
        yield return new WaitForSeconds(2.5f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  We’ll send you a list shortly with everything you'll need to maximize your survival chances.", "#24c1ff");
        yield return new WaitForSeconds(4f);
        TerminalUI.Instance.AddToTerminal("CAPCOM:  Which, of course, is… 100%", "#24c1ff");
        yield return new WaitForSeconds(4f);

        //Flight director assigns next task
        yield return new WaitForSeconds(2f);
        SoundManager.Instance.Play("beep");
        TerminalUI.Instance.AddToTerminal("<color=#ff33f8>Flight Director:</color> <color=green>@MissionOperationsSpecialist</color> <color=#ff33f8> prioritize and optimize resources for Martinez, keeping weight constraints in mind. Send him the list immediately.</color>");
        yield return new WaitForSeconds(3.5f);
        TerminalUI.Instance.AddToTerminal("Open the cargo-bay-camera and maximise survival chance!", "green");
        MainScreen.Instance.SetState(10); //show knapsack game

    }

    //8 KNAPSACK GAME COMPLETED

    public void onKnapsackCompleted()
    {
        StartCoroutine(KnapsackCompletedScenario());
    }

    private IEnumerator KnapsackCompletedScenario()
    {
        SoundManager.Instance.Play("correct");
        TerminalUI.Instance.AddToTerminal("Cargo game completed!", "green");
        MainScreen.Instance.SetState(0); //show tue bg
        yield return new WaitForSeconds(3f);
        TerminalUI.Instance.AddToTerminal("Martinez: Send me the list of items!", "yellow");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("type 'send list.txt' to send item list to Martinez");

        waitingForList = true;

    }

    //9 KNAPSACK SENT

    public void onKnapsackSent()
    {
        StartCoroutine(KnapsackSentScenario());
    }

    private IEnumerator KnapsackSentScenario()
    {
        //Reaction to list sent
        yield return new WaitForSeconds(2f);
        TerminalUI.Instance.AddToTerminal("Martinez: Got it!", "yellow");
        yield return new WaitForSeconds(2f);
        TerminalUI.Instance.AddToTerminal("Martinez: Packing now. Is there anything else?", "yellow");
        yield return new WaitForSeconds(4f);
        SoundManager.Instance.Play("voiceover5");
        yield return new WaitForSeconds(1f);
        TerminalUI.Instance.AddToTerminal("CAPCOM: Martinez, head to the escape pod with the gear. You have 20 minutes to collect everything and get yourself ready. You’re coming home.", "#24c1ff");
        yield return new WaitForSeconds(7f);
    }
    
    //--------------------------------------------------------------------------------
    //COMMANDS/HELP FUNCTIONS

    // OPEN GATE COMMAND
    // STARTS SCENARIO 3 - BREAKDOWN
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
            SoundManager.Instance.Play("error");
            return;
        }

        // Check gate ID
        if (gateId == "A")
        {
            gateAOpened = true;
            waitingForGate = false;
            TerminalUI.Instance.AddToTerminal("Gate A is now open!");
            SoundManager.Instance.Play("correct");
            // Possibly do more logic after the gate is open...
        }
        else
        {
            TerminalUI.Instance.AddToTerminal($"Gate '{gateId}' not recognized. Check the map!");
            SoundManager.Instance.Play("error");
        }
    }

    public void OnSendDiagramCommand(string toSend)
    {
        if (!gameStarted)
        {
            TerminalUI.Instance.AddToTerminal("You need to start the game first. Type 'start'.");
            return;
        }

        if (!waitingForDiagram && !waitingForFlow && !waitingForList)
        {
            TerminalUI.Instance.AddToTerminal("Nothing to send at this moment.");
            SoundManager.Instance.Play("error");
            return;
        }

        if (waitingForDiagram && toSend == "diagram.pdf")
        {
            waitingForDiagram = false;
            TerminalUI.Instance.AddToTerminal("Diagram sent!");
            SoundManager.Instance.Play("correct");
            onDiagramSent();
        }
        else if (waitingForFlow && toSend == "flow.pdf")
        {
            waitingForFlow = false;
            TerminalUI.Instance.AddToTerminal("Diagram sent!");
            SoundManager.Instance.Play("correct");
            onFlowSent();
        }
        else if (waitingForList && toSend == "list.txt")
        {
            waitingForList = false;
            TerminalUI.Instance.AddToTerminal("List sent!");
            SoundManager.Instance.Play("correct");
            onKnapsackSent();
        }
        else
        {
            TerminalUI.Instance.AddToTerminal($"File '{toSend}' not recognized. Follow instructions precisely");
            SoundManager.Instance.Play("error");
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
