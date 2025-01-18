using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class ConnectionManager : MonoBehaviour
{
    public GameObject andGate, orGate, notGate;
    public GameObject s1, s2, s3, s4, s5; // Switches
    public SpriteRenderer l1, l2, l3;    // Lights as SpriteRenderer
    

    private Color lightOnColor = Color.yellow; // Color for "on" state
    private Color lightOffColor = Color.gray;  // Color for "off" state


    public void ValidateConnections()
    {
        // Check AND Gate
        bool isAndGateValid = IsInputConnected(andGate, "InputPort1", s1) &&
                              IsInputConnected(andGate, "InputPort2", s3) &&
                              IsOutputConnected(andGate, l1);
        UpdateLightState(l1, isAndGateValid);

        // Check OR Gate
        bool isOrGateValid = (IsInputConnected(orGate, "InputPort1", s2) ||
                              IsInputConnected(orGate, "InputPort2", s4)) &&
                              IsOutputConnected(orGate, l2);
        UpdateLightState(l2, false);

        // Check NOT Gate
        bool isNotGateValid = IsInputConnected(notGate, "InputPort", s5) &&
                              IsOutputConnected(notGate, l3);
        UpdateLightState(l3, isNotGateValid);

       
    }

    

    private void UpdateLightState(SpriteRenderer light, bool isOn)
    {
        if (light != null)
        {
            light.color = isOn ? lightOnColor : lightOffColor;
        }
    }

    private bool IsInputConnected(GameObject gate, string inputPortName, GameObject switchObj)
    {
        Transform inputPort = gate.transform.Find(inputPortName);
        if (inputPort == null)
        {
            Debug.LogWarning($"Input port '{inputPortName}' not found on {gate.name}");
            return false;
        }

        Wire[] wires = FindObjectsOfType<Wire>();
        foreach (Wire wire in wires)
        {
            if (wire.endPoint == inputPort || wire.startPoint == switchObj.transform)
            {
                Debug.Log($"{switchObj.name} is connected to {inputPortName} of {gate.name}");
                return true;
            }
        }

        return false;
    }

    private bool IsOutputConnected(GameObject gate, SpriteRenderer light)
    {
        Transform outputPort = gate.transform.Find("OutputPort");
        if (outputPort == null) return false;

        Wire[] wires = FindObjectsOfType<Wire>();
        foreach (Wire wire in wires)
        {
            if (wire.startPoint == outputPort || wire.endPoint == light.transform)
                return true;
        }
        return false;
    }

    public void OnWireConnected()
    {
        ValidateConnections();
    }
}