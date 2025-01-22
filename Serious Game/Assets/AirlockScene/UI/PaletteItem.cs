using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PaletteItem : DraggableUI
{
    // We can also add additional references if needed, 
    // e.g. a prefab or a "default parent" for the new item
    private Transform paletteDropParent;  // Where new items go
    private GameObject copy;

    protected override void Awake()
    {
        base.Awake();
        
        Transform paletteParent = transform.parent;

        if (paletteParent != null && paletteParent.name == "Palette")
        {
            // Find the sibling named 'Commands'
            Transform commandsParent = paletteParent.parent.Find("Commands");
            if (commandsParent != null)
            {
                paletteDropParent = commandsParent;
            }
            else
            {
                Debug.LogError("Commands Transform not found as a sibling of Palette.");
            }
        }
        else
        {
            Debug.LogError("Palette Transform not found as the parent.");
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag (PaletteItem)");

        // Create copy of this item in the same parent
        copy = Instantiate(gameObject, transform.parent);

        // Because we cloned the entire GameObject (including this script),
        // remove the PaletteItem script from the copy:
        Destroy(copy.GetComponent<PaletteItem>());
        // Add the DragItem script so that the copy can be dragged around.
        copy.AddComponent<DragItem>();
        

        // Typically you’d want to do more setup on the copy (e.g. initial positions)
        copy.transform.SetParent(transform.root, true);
        copy.transform.SetAsLastSibling();
        
        // Make sure the copy doesn't block raycasts while dragging
        var copyCanvasGroup = copy.GetComponent<CanvasGroup>();
        if (copyCanvasGroup == null)
            copyCanvasGroup = copy.AddComponent<CanvasGroup>();
        copyCanvasGroup.blocksRaycasts = false;

        // Fade out or partially hide the original "palette" item
        SetPartialAlpha(0.7f);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (copy == null) return;
        Debug.Log("Dragging (PaletteItem)");
        copy.transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (copy == null) return;
        Debug.Log("End Drag (PaletteItem)");

        // If you need the new item to be parented somewhere specific:
        if (paletteDropParent != null)
            copy.transform.SetParent(paletteDropParent);
            // Figure out new sibling index to reorder in the parent
            int newSiblingIndex = paletteDropParent.childCount;
            for (int i = 0; i < paletteDropParent.childCount; i++)
            {
                if (transform.position.y > paletteDropParent.GetChild(i).position.y)
                {
                    newSiblingIndex = i;
                    break;
                }
            }
            copy.transform.SetSiblingIndex(newSiblingIndex);

        // Re-enable raycasts on the clone
        var copyCanvasGroup = copy.GetComponent<CanvasGroup>();
        if (copyCanvasGroup)
            copyCanvasGroup.blocksRaycasts = true;

        SetPartialAlpha(1f);
        // Destroy the original palette item if that’s desired
        //Destroy(gameObject);
    }
}
