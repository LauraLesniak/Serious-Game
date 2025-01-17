using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles the logic for managing flow through a network of pipes.
// Tracks pipe flows, capacities, and available flows at different nodes.
public class MaxFlowLogic : MonoBehaviour
{
    // Indicates whether the flow in each pipe is increasing or decreasing.
    public bool[] increasing = {true, true, true, true, true, true, true, true, true, true}; 

    // Current flow values for each pipe, flows can change.
    public int[] pipeFlows = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    // Maximum flow capacity for each pipe, capacities are fixed.
    public static int[] pipeCapacities = { 2, 3, 3, 2, 6, 1, 2, 3, 5, 5 }; 

    // Available flow at each corner node in the network.
    public int[] availableFlow = { 1000, 0, 0, 0, 0, 0, 0 };

    // Maximum flow achieved at the sink (corner T = index 1).
    private int maxFlowAchieved = 0; 

    // TextMeshPro component displaying the maximum flow achieved at the sink.
    public TextMeshPro maxFlowText;

    // TextMeshPro component displaying the current flow at the sink.
    public TextMeshPro currentFlowText; 

    // Defines input and output corners for each pipe.
    public int[,] connections =
    {
        { 0, 2 }, { 2, 3 }, { 2, 5 }, { 3, 1 },
        { 0, 4 }, { 4, 5 }, { 5, 1 }, { 6, 0 },
        { 4, 6 }, { 6, 1 }
    };

    // Attempts to adjust the flow for a specified pipe.
    // If the flow can be increased or decreased, updates the flow accordingly.
    // Returns true if the flow was successfully adjusted; otherwise, false.
    public bool TryAdjustFlow(int pipeID)
    {
        // Checks if the pipeID is valid.
        if (pipeID < 0 || pipeID >= pipeFlows.Length)
        {
            Debug.LogError($"Invalid pipeID: {pipeID}");
            return false;
        }

        // Flow gets to be increased.
        else if ((increasing[pipeID] && CanIncrease(pipeID)) 
                || (!increasing[pipeID] && !CanDecrease(pipeID) && CanIncrease(pipeID)))
        {
            Increase(pipeID);
            UpdateFlowText();
            return true;
        }

        // Flow gets to be decreased.
        else if ((increasing[pipeID] && !CanIncrease(pipeID) && CanDecrease(pipeID)) 
                || (!increasing[pipeID] && CanDecrease(pipeID)))
        { 
            Decrease(pipeID);
            UpdateFlowText();
            return true;
        }

        return false; // Flow wasn't adjusted.
    }

    // Checks if the flow for a specific pipe can be increased.
    bool CanIncrease(int pipeID) {
        return availableFlow[connections[pipeID, 0]] > 0 
                && pipeFlows[pipeID] < pipeCapacities[pipeID];
    }

    // Checks if the flow for a specific pipe can be decreased.
    bool CanDecrease(int pipeID) {
        return availableFlow[connections[pipeID, 1]] > 0 
                && pipeFlows[pipeID] > 0;
    }

    // Increases the flow for a specific pipe and updates the network's flow state.
    void Increase(int pipeID) {
        increasing[pipeID] = true;
        pipeFlows[pipeID]++;
        availableFlow[connections[pipeID, 0]] --;
        availableFlow[connections[pipeID, 1]] ++;
    }

    // Decreases the flow for a specific pipe and updates the network's flow state.
    void Decrease(int pipeID) {
        increasing[pipeID] = false;
        pipeFlows[pipeID]--;
        availableFlow[connections[pipeID, 1]] --;
        availableFlow[connections[pipeID, 0]] ++;
    }

    // Updates the caption for max and current flow text.
    void UpdateFlowText()
    {
        // Check if the current flow at the sink (corner index 1) exceeds maxFlowAchieved
        if (availableFlow[1] > maxFlowAchieved)
        {
            maxFlowAchieved = availableFlow[1];
            if (maxFlowText != null)
            {
                maxFlowText.text = $"Max flow achieved: {maxFlowAchieved}";
            }
        }
        if (currentFlowText != null)
        {
            currentFlowText.text = $"Current flow: {availableFlow[1]}";
        }
    }

    // Gets the flow text for a specific pipe, formatted as "current/maximum".
    public string GetFlowText(int pipeID)
    {
        return $"{pipeFlows[pipeID]}/{pipeCapacities[pipeID]}";
    }
}
