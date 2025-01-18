using UnityEngine;

public class Wire : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Initialize with two points
    }

    public void SetStartPoint(Transform start)
    {
        startPoint = start;
        lineRenderer.SetPosition(0, start.position);
        Debug.Log($"Wire start set to: {start.name} ({start.position})");
    }

    public void SetEndPoint(Transform end)
    {
        endPoint = end;
        lineRenderer.SetPosition(1, end.position);
        Debug.Log($"Wire end set to: {end.name} ({end.position})");
    }


    public void UpdateEndPosition(Vector3 position)
    {
        lineRenderer.SetPosition(1, position); // Update the wire's end position dynamically
    }
}