using UnityEngine;
using UnityEngine.EventSystems;

public class WireInteraction : MonoBehaviour, IPointerClickHandler
{
    public bool isInputPort; // True for gate inputs, false for gate outputs
    private WireManager wireManager;

    private void Start()
    {
        wireManager = Object.FindFirstObjectByType<WireManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (wireManager == null) return;

        if (isInputPort)
        {
            // If this is an input port, end the wire
            wireManager.EndWire(transform);
        }
        else
        {
            // If this is an output port, start a new wire
            wireManager.StartWire(transform);
        }
    }
}
