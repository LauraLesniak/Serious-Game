using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isLocked = false;  // Flag to check if the gate is locked

    private void OnMouseDown()
    {
        // Prevent dragging if the gate is locked
        if (isLocked) return;

        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Adjust based on camera depth
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // This method can be called to lock the gate after it's connected
    public void LockGate()
    {
        isLocked = true;
    }

    // This method can be called to unlock the gate if needed
    public void UnlockGate()
    {
        isLocked = false;
    }
}
