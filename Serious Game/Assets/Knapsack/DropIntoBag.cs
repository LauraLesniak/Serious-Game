using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropIntoBag : MonoBehaviour
{
    public Transform snapPositionParent;   // Parent object where items snap
    public float bagWeightLimit = 10f;     // Total weight limit of the bag
    public int maxPriority = 20;
    public TMP_Text weightText;            // UI Text to display total weight
    public TMP_Text survivalChanceText;
    

    private float totalPriority = 0f;        // Current total weight
    private float totalWeight = 0f;        // Current total weight
    private int snapIndex = 0;             // Index for snap positions

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is draggable
        if (other.CompareTag("Draggable"))
        {
            DraggableWeight weightScript = other.GetComponent<DraggableWeight>();

            if (weightScript != null)
            {
                // Check if we can add this object's weight
                if (totalWeight + weightScript.weight <= bagWeightLimit)
                {
                    // Update total weight
                    totalWeight += weightScript.weight;
                    totalPriority += weightScript.priority; // Update total priority

                    // Snap object to the next position
                    if (snapIndex < snapPositionParent.childCount)
                    {
                        Transform snapPosition = snapPositionParent.GetChild(snapIndex);
                        other.transform.position = snapPosition.position;
                        other.transform.SetParent(snapPositionParent);
                        snapIndex++;
                    }
                    else
                    {
                        Debug.Log("No more snap positions available.");
                    }

                    // Update UI
                    UpdateUI();
                    weightText.text = "Item added: " + weightScript.weight + " | Total Weight: " + totalWeight;
                }
                else
                {
                    weightScript.Respawn();
                    weightText.text = "Bag is full! Cannot add item.";
                    Debug.Log("Bag is full! Weight limit exceeded.");
                }
            }
            else
            {
                Debug.LogError("DraggableWeight component is missing on the object: " + other.name);
            }
        }
        else
        {
            Debug.LogWarning("Object is not tagged as Draggable: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object is draggable and already inside the bag
        if (other.CompareTag("Draggable") && other.transform.parent == snapPositionParent)
        {
            DraggableWeight weightScript = other.GetComponent<DraggableWeight>();

            if (weightScript != null)
            {
                // Subtract the object's weight from the total
                totalWeight -= weightScript.weight;
                totalPriority -= weightScript.priority; // Update total priority

                // Detach the object from the snap position parent
                other.transform.SetParent(null);

                // Update snap index to allow reuse of the position
                snapIndex = Mathf.Max(0, snapIndex - 1);

                // Update UI
                UpdateUI();
                weightText.text = "Item removed: " + weightScript.weight + " | Total Weight: " + totalWeight;
            }
            else
            {
                Debug.LogError("DraggableWeight component is missing on the object: " + other.name);
            }
        }
    }

    private void UpdateSurvivalChance()
    {
        if (survivalChanceText == null)
        {
            Debug.LogError("SurvivalChanceText is not assigned in the Inspector.");
            return;
        }

        if (maxPriority == 0)
        {
            Debug.LogWarning("Max Priority is zero. Cannot calculate survival chance.");
            survivalChanceText.text = "Chance of Survival: 0%";
            return;
        }

        float survivalChance = ((float)totalPriority / maxPriority) * 100f;
        survivalChanceText.text = $"Chance of Survival: {survivalChance:F1}%";
        Debug.Log($"Survival Chance Calculated: {survivalChance}% (Total Priority: {totalPriority}, Max Priority: {maxPriority})");
    }

    private void UpdateUI()
    {
        UpdateSurvivalChance();
        if (weightText != null)
        {
            weightText.text = "Total Weight: " + totalWeight.ToString("F1") + " / " + bagWeightLimit;
        }
    }
}
