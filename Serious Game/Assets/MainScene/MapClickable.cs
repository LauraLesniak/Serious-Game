using TMPro;
using UnityEngine;

public class MapClickable : Clickable
{
    public GameObject map; // Reference to the map GameObject

    private new void Awake()
    {
        base.Awake();
        if (map != null)
        {
            map.SetActive(false); // Ensure the map is initially hidden
        }
    }

    // Override the OnMouseDown method to open the map
    protected override void OnMouseDown()
    {
        if (TerminalUI.Instance.terminalActive)
        {
            return;
        }

        // Open the map if it's not already active
        if (map != null && !map.activeSelf)
        {
            map.SetActive(true);
        }

        // Optionally, you can still play the sound
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    }
}