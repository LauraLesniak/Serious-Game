using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class ShowMonitor : MonoBehaviour
{
    [SerializeField] private int monitorID; // The ID that determines which scene to load
    private TerminalUI terminal;
    private LineRenderer lineRenderer;
    public TextMeshPro text;

    void Start()
    {
        //find Terminal in scene
        terminal = FindObjectOfType<TerminalUI>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        text.enabled = false;
    }

    // Called when the object is clicked
    void OnMouseDown()
    {
        if (TerminalUI.Instance.terminalActive)
        {
            return;
        }
        switch (monitorID)
        {
            case 1:
                // find a gameobject with the name CameraCanvas
                //set its first children active to show it,
                //run the function U
                CameraManager.Instance.ShowCanvas();

                break;

            case 2:
                // Load the current level's scene based on its build index
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //SceneManager.LoadScene("AirlockScene");
                MainScreen.Instance.OnPress();
                break;

            case 3:
                // Load Scene B
                terminal.ShowTerminal();
                //SceneManager.LoadScene("TerminalScene");
                //FakeTerminal.Instance.ShowTerminal(); // Ensure the terminal shows when returning
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
