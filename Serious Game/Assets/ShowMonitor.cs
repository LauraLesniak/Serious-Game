using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class ShowMonitor : MonoBehaviour
{
    [SerializeField] private int monitorID; // The ID that determines which scene to load
    private FakeTerminal terminal;
    private LineRenderer lineRenderer;
    private TextMeshPro text;

    void Start()
    {
        //find Terminal in scene
        terminal = FindObjectOfType<FakeTerminal>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        text = GetComponentInChildren<TextMeshPro>();
        text.enabled = false;
    }

    // Called when the object is clicked
    void OnMouseDown()
    {
        switch (monitorID)
        {
            case 1:
                // Load the scene with the map of the level
                //SceneManager.LoadScene("map");
                break;

            case 2:
                // Load the current level's scene based on its build index
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene("AirlockScene");
                break;

            case 3:
                // Load Scene B
                terminal.ShowTerminal();
                //SceneManager.LoadScene("TerminalScene");
                //FakeTerminal.Instance.ShowTerminal(); // Ensure the terminal shows when returning
                break;
            case 4:
                //Click away
                terminal.HideTerminal();
                break;
            default:
                // Handle an invalid monitorID
                Debug.LogWarning("Invalid monitorID! No scene to load.");
                break;
        }
    }

    void OnMouseEnter()
    {
        text.enabled = true;
        lineRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        lineRenderer.enabled = false;
        text.enabled = false;
    }
}
