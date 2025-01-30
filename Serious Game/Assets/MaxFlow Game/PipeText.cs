using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles interactions with the pipe and displays its flow information.
public class PipeText : MonoBehaviour
{
    // The ID of the pipe this script controls.
    public int pipeID;

    // Reference to the MaxFlowLogic script controlling the network logic.
    private MaxFlowLogic maxFlowLogic;

    // TextMeshPro component displaying the current flow for the pipe.
    public TextMeshPro flowText;

    // Initializes the pipe, finds the MaxFlowLogic instance, and updates the flow text.
    void Start()
    {
        maxFlowLogic = FindObjectOfType<MaxFlowLogic>();
        if (maxFlowLogic == null)
        {
            Debug.LogError("MaxFlowLogic component not found in the scene.");
            return;
        }

        UpdateFlowText();
    }

    // Handles user interaction with the pipe by adjusting its flow when clicked.
    void OnMouseDown()
    {
        // Attempt to adjust the flow for this pipe.
        if (maxFlowLogic.TryAdjustFlow(pipeID))
        {
            UpdateFlowText();
            SoundManager.Instance.Play("pipe");
        }
        else
        {
            Debug.Log($"Pipe {pipeID}: Flow unchanged.");
            SoundManager.Instance.Play("error");
        }
    }

    // Updates the text for the corresponding pipe.
    void UpdateFlowText()
    {
        if (flowText != null)
        {
            flowText.text = maxFlowLogic.GetFlowText(pipeID);
        }
    }
}
