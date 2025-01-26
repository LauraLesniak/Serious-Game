using UnityEngine;

// Ensures we have the needed components
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Clickable : MonoBehaviour
{
    private AudioSource audioSource;
    private LineRenderer lineRenderer;
    //public TextMeshPro text;

    private void Awake()
    {
        // Grab the AudioSource on this same GameObject
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        // text.enabled = false;
    }

    // This gets called automatically on left-click if the object
    // has a 2D collider and the camera is looking at it.
    private void OnMouseDown()
    {
        if (FakeTerminal.Instance.terminalActive)
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
        // text.enabled = true;
        lineRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        lineRenderer.enabled = false;
        // text.enabled = false;
    }
}
