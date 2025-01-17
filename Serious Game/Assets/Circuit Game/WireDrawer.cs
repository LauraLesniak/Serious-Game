using UnityEngine;

public class WireDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

        // Initialize line with two points
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }

    void Update()
    {
        // Update the second point to follow the mouse position
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep the wire in 2D space
        lineRenderer.SetPosition(1, mousePosition);

        // Place logic here to snap the wire to a target when releasing the mouse
        if (Input.GetMouseButtonUp(0)) // Left mouse button release
        {
            // Example: Snap to a target
            // Replace "targetPosition" with your snapping logic
            Vector3 targetPosition = new Vector3(1, 1, 0);
            lineRenderer.SetPosition(1, targetPosition);
        }
    }
}