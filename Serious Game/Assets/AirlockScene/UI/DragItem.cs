using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragItem : DraggableUI
{
    // If you need specific Awake logic for DragItem, you can override,
    // but don't forget to call base.Awake() if you still want that text logic!
    protected override void Awake()
    {
        base.Awake(); 
        // ... additional DragItem-specific initialization ...
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag (DragItem)");

        // Save reference to the parent before we take it out of the layout
        parentAfterDrag = transform.parent;

        // Move to the top so it's not clipped by other UI elements
        transform.SetParent(transform.root, true);
        transform.SetAsLastSibling();

        // Prevent text from blocking raycasts while dragging
        if (text != null)
            text.raycastTarget = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging (DragItem)");
        // Follow mouse
        transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag (DragItem)");

        //if dragged too much to the right, delete it
        if (transform.position.x > Screen.width * 0.2f)
        {
            Destroy(gameObject);
            return;
        }

        // Figure out new sibling index to reorder in the parent
        int newSiblingIndex = parentAfterDrag.childCount;
        for (int i = 0; i < parentAfterDrag.childCount; i++)
        {
            if (transform.position.y > parentAfterDrag.GetChild(i).position.y)
            {
                newSiblingIndex = i;
                break;
            }
        }
        transform.SetParent(parentAfterDrag);
        transform.SetSiblingIndex(newSiblingIndex);

        // Restore text's ability to block raycasts
        if (text != null)
            text.raycastTarget = true;
    }
}
