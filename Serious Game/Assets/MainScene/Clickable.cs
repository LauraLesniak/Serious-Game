using TMPro;
using UnityEngine;

// Ensures we have the needed components
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Clickable : MonoBehaviour
{
    protected AudioSource audioSource;
    private LineRenderer lineRenderer;
    public TextMeshPro text;
    //public TextMeshPro text;

    protected void Awake()
    {
        // Grab the AudioSource on this same GameObject
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        if (text != null)
        {
            text.enabled = false;
        }
    }

    // This gets called automatically on left-click if the object
    // has a 2D collider and the camera is looking at it.
    protected virtual void OnMouseDown()
    {
        if (TerminalUI.Instance.terminalActive)
        {
            return;
        }
        // Play beep if AudioSource is available
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    }

    void OnMouseEnter()
    {
        if (text != null)
        {
            text.enabled = true;
        }
        lineRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        lineRenderer.enabled = false;
        if (text != null)
        {
            text.enabled = false;
        }
    }
}
