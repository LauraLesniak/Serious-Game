using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropIntoBag : MonoBehaviour
{
    public Transform snapPositionParent;   // Parent object where items snap
    public Transform items;   // Parent items are
    public float bagWeightLimit = 10f;     // Total weight limit of the bag
    public int maxPriority = 20;
    public TMP_Text weightText;            // UI Text to display total weight
    public TMP_Text survivalChanceText;

    private float totalPriority = 0f;      // Current total priority
    private float totalWeight = 0f;        // Current total weight
    private int snapIndex = 0;             // Index for snap positions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Draggable"))
        {
            DraggableWeight weightScript = other.GetComponent<DraggableWeight>();

            if (weightScript != null)
            {
                if (totalWeight + weightScript.weight <= bagWeightLimit)
                {
                    totalWeight += weightScript.weight;
                    totalPriority += weightScript.priority;

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

                    UpdateUI();
                    weightText.text = "Item added: " + weightScript.weight + "kg | Total Weight: " + totalWeight + "kg / " + bagWeightLimit + "kg";
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

    public void ResetBag()
    {
    // Reset weights and priorities
    totalWeight = 0f;
    totalPriority = 0f;
    snapIndex = 0;

    // Loop through all items
    foreach (Transform item in items)
    {
        DraggableWeight weightScript = item.GetComponent<DraggableWeight>();

        if (weightScript != null)
        {
            weightScript.Respawn(); // Send the item back to its original position
        }
    }

    // Update the UI
    UpdateUI();
    weightText.text = "Bag has been reset!";
    Debug.Log("Bag and items have been reset!");
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

        if (survivalChance >= 100f)
        {
            GameObject uiObject = GameObject.Find("Completed");
                if (uiObject != null)
                {
                    //set the children as active
                    foreach (Transform child in uiObject.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                ScenarioManager.Instance.onKnapsackCompleted();
        }
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
