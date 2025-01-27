using UnityEngine;

public class WireManager : MonoBehaviour
{
    public GameObject wirePrefab; // Assign the wire prefab in the Inspector
    private Wire currentWire;    // The current wire being drawn
    private bool isDrawing = false; // Is the player currently drawing a wire?

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On left mouse click
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Switch") || hit.collider.CompareTag("OutputPort"))
                {
                    StartWire(hit.collider.transform); // Start the wire from the clicked object
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isDrawing) // On mouse release
        {
            int inputIOLayer = LayerMask.NameToLayer("Gate IO");
            int layerMask = 1 << inputIOLayer;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
            Debug.Log(hit.collider);
            if (hit.collider != null && currentWire != null)
            {
                if (hit.collider.CompareTag("InputPort"))
                {
                    EndWire(hit.collider.transform); // End the wire at the port
                }
                else
                {
                    // Cancel the wire if released on an invalid target
                    Destroy(currentWire.gameObject);
                    currentWire = null;
                    isDrawing = false;
                }
            }
            
            if (hit.collider == null)
            {
                Destroy(currentWire.gameObject);
                currentWire = null;
                isDrawing = false;
            }
        }

        if (isDrawing && currentWire != null) // Update wire position while dragging
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Keep wire in 2D space
            currentWire.UpdateEndPosition(mousePos);
        }
    }

    public void StartWire(Transform startPoint)
    {
        currentWire = Instantiate(wirePrefab).GetComponent<Wire>();
        currentWire.SetStartPoint(startPoint); // Set the starting point of the wire
        isDrawing = true;
    }

    public void EndWire(Transform endPoint)
    {
        if (currentWire != null && endPoint != null)
        {
            currentWire.SetEndPoint(endPoint); // Set the ending point of the wire
            currentWire = null;
            isDrawing = false;

            // Lock the gate once it has been connected to a wire
            if (endPoint.CompareTag("InputPort") || endPoint.CompareTag("Light"))
            {
                // Lock the gate that the wire is connected to (input port)
                Draggable gate = endPoint.transform.parent.GetComponent<Draggable>();
                if (gate != null)
                {
                    gate.LockGate(); // Lock the gate
                }

                ConnectionManager connectionManager = FindObjectOfType<ConnectionManager>();
                connectionManager?.OnWireConnected();
            }
        }
        else
        {
            // If the wire is invalid, destroy it
            Destroy(currentWire?.gameObject);
            isDrawing = false;
        }
    }
}