using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public GameObject andGate, orGate, notGate;
    public GameObject s1, s2, s3, s4, s5; // Switches
    public SpriteRenderer l1, l2, l3;    // Lights as SpriteRenderer
    public Transform l1InputPort, l2InputPort, l3InputPort; // Input ports of lights

    private Color lightOnColor = Color.yellow; // Color for "on" state
    private Color lightOffColor = Color.gray;  // Color for "off" state

    public bool gameCompleted = false;

    public void ValidateConnections()
    {
        // Check if switches are correctly connected to the gates
        bool isSwitchesConnectedToAndGate = CheckSwitchGateConnection(andGate, s1, s3);
        bool isSwitchesConnectedToOrGate = CheckSwitchGateConnection(orGate, s2, s4);
        bool isSwitchesConnectedToNotGate = CheckSwitchGateConnectionForNotGate(notGate, s5);

        // Check if each gate's output port is connected to a light's input port
        bool isAndGateConnectedToLight = IsOutputConnected(andGate, "OutputPort", l1InputPort);
        bool isOrGateConnectedToLight = IsOutputConnected(orGate, "OutputPort", l2InputPort);
        bool isNotGateConnectedToLight = IsOutputConnected(notGate, "OutputPort", l3InputPort);

        // Turn on the lights if gates are connected to them
        UpdateLightState(l1, isSwitchesConnectedToAndGate && isAndGateConnectedToLight);
        UpdateLightState(l2, isSwitchesConnectedToOrGate && isOrGateConnectedToLight);
        UpdateLightState(l3, isSwitchesConnectedToNotGate && isNotGateConnectedToLight);

        //if all lights are on, show the completed text
        if (!gameCompleted)
        {
            if (l1.color == lightOnColor && l2.color == lightOnColor && l3.color == lightOnColor)
            {
                gameCompleted = true;
                GameObject completed = GameObject.Find("Completed");
                if (completed.transform.childCount > 0)
                {
                    completed.transform.GetChild(0).gameObject.SetActive(true);
                }
                ScenarioManager.Instance.onCircuitGameCompleted();
            }
        }
    }

    private void UpdateLightState(SpriteRenderer light, bool isOn)
    {
        light.color = isOn ? lightOnColor : lightOffColor;
    }

    #region Switch to Gate Connections

    private bool CheckSwitchGateConnection(GameObject gate, GameObject switchObj1, GameObject switchObj2)
    {
        bool isSwitch1Connected = IsInputConnected(gate, "InputPort1", switchObj1);
        bool isSwitch2Connected = IsInputConnected(gate, "InputPort2", switchObj2);

        return isSwitch1Connected && isSwitch2Connected;
    }

    private bool CheckSwitchGateConnectionForNotGate(GameObject gate, GameObject switchObj)
    {
        // The NOT gate only has one input, so we check that one input is connected
        return IsInputConnected(gate, "InputPort", switchObj);
    }

    #endregion

    #region Helper Methods

    // Check if the input port of the gate is connected to the specific switch
    private bool IsInputConnected(GameObject gate, string inputPortName, GameObject switchObj)
    {
        Transform inputPort = gate.transform.Find(inputPortName);
        if (inputPort == null)
        {
            Debug.LogWarning($"Input port '{inputPortName}' not found on {gate.name}");
            return false;
        }

        // Look for wires that connect the switch to the input port
        Wire[] wires = FindObjectsOfType<Wire>();
        foreach (Wire wire in wires)
        {
            if ((wire.startPoint == switchObj.transform && wire.endPoint == inputPort) ||
                (wire.endPoint == switchObj.transform && wire.startPoint == inputPort))
            {
                return true;
            }
        }

        return false;
    }

    // Check if the output port of the gate is connected to the specific light's input port
    private bool IsOutputConnected(GameObject gate, string outputPortName, Transform lightInputPort)
    {
        Transform outputPort = gate.transform.Find(outputPortName);
        if (outputPort == null)
        {
            Debug.LogWarning($"Output port '{outputPortName}' not found on {gate.name}");
            return false;
        }

        // Look for wires that connect the output port to the light's input port
        Wire[] wires = FindObjectsOfType<Wire>();
        foreach (Wire wire in wires)
        {
            if ((wire.startPoint == outputPort && wire.endPoint == lightInputPort) ||
                (wire.endPoint == outputPort && wire.startPoint == lightInputPort))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    // Method to be called when a wire is connected
    public void OnWireConnected()
    {
        ValidateConnections();
    }
}
