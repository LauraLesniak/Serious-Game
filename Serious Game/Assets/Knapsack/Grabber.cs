using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; 

public class Grabber : MonoBehaviour
{
    private GameObject selectedObject;
    private Rigidbody selectedRigidbody;
    private Plane dragPlane;
    private Vector3 offset;
    private Camera mainCamera;

    public Transform bagCenter;
    public float bagWeightLimit = 25f;
    public TMP_Text weightText;
    public TMP_Text survivalChanceText;

    private HashSet<GameObject> objectsInBag = new HashSet<GameObject>();

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TrySelectObject();

        if (Input.GetMouseButton(0) && selectedObject != null)
            DragObject();

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
            DropObject();
    }

    private void TrySelectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Draggable"))
            {
                selectedObject = hit.collider.gameObject;
                selectedRigidbody = selectedObject.GetComponent<Rigidbody>();

                dragPlane = new Plane(Vector3.up, selectedObject.transform.position);

                float distance;
                dragPlane.Raycast(ray, out distance);
                offset = selectedObject.transform.position - ray.GetPoint(distance);

                selectedRigidbody.isKinematic = true;
                Cursor.visible = false;

                Debug.Log($"Selected: {selectedObject.name}");
            }
        }
    }

    private void DragObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance) + offset;
            selectedObject.transform.position = targetPosition;
        }
    }

    private void DropObject()
    {
        Collider bagCollider = bagCenter.GetComponent<Collider>();
        if (bagCollider == null)
        {
            Debug.LogError("BagCenter is missing a Collider component!");
            return;
        }

        Vector3 objectPosition = selectedObject.transform.position;
        DraggableWeight weightScript = selectedObject.GetComponent<DraggableWeight>();

        if (weightScript == null)
        {
            Debug.LogError("Dropped object is missing DraggableWeight component!");
            return;
        }

        Debug.Log($"Attempting to drop object: {selectedObject.name}");

        // If the object is dropped inside the bag
        if (bagCollider.bounds.Contains(objectPosition))
        {
            if (!objectsInBag.Contains(selectedObject))// && totalWeight + weightScript.weight <= bagWeightLimit)
            {
                objectsInBag.Add(selectedObject);

                // Reposition inside the bag
                selectedObject.transform.position = bagCenter.position + Vector3.up * (0.5f * objectsInBag.Count);
                selectedObject.transform.SetParent(bagCenter);

            }
        }
        else if (objectsInBag.Contains(selectedObject))
        {
            objectsInBag.Remove(selectedObject);

            selectedObject.transform.SetParent(null);
        }

        selectedRigidbody.isKinematic = false;
        selectedObject = null;
        selectedRigidbody = null;
        Cursor.visible = true;
    }

}
