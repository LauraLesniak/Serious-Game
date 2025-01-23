using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnifiedDraggableItem : DraggableUI
{
    [SerializeField]
    public bool isPaletteItem = false; // Flag to differentiate between PaletteItem and DragItem
    private GameObject copy;
    private Transform divider;

    protected override void Awake()
    {
        base.Awake();
        // Additional initialization if needed
        GameObject uiObject = GameObject.Find("Commands");
        if (uiObject != null)
        {
            parentAfterDrag = uiObject.transform;
            divider = parentAfterDrag.Find("Divider");
            if (divider == null)
            {
                Debug.LogError("Divider object not found under Commands.");
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag (UnifiedDraggableItem)");

        // Create a copy of this item
        copy = Instantiate(gameObject, transform.parent);
        // Set up the copy
        copy.transform.SetParent(transform.root, true);
        copy.transform.SetAsLastSibling();

        if (isPaletteItem)
        {
            copy.GetComponent<UnifiedDraggableItem>().isPaletteItem = false;
        }
        else
        {
            gameObject.transform.SetParent(null);
        }
        
        // Show the divider
        if (divider != null)
        {
            divider.gameObject.SetActive(true);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging (UnifiedDraggableItem)");

        if (copy != null)
        {
            copy.transform.position = Input.mousePosition;
        }

        // Move the divider to indicate the new position
        if (divider != null)
        {
            if (Input.mousePosition.x > Screen.width * 0.3f)
            {
                divider.gameObject.SetActive(false);
            }
            else
            {
                divider.gameObject.SetActive(true);
                int newSiblingIndex = parentAfterDrag.childCount;
                for (int i = 0; i < parentAfterDrag.childCount; i++)
                {
                    if (Input.mousePosition.y > parentAfterDrag.GetChild(i).position.y)
                    {
                        newSiblingIndex = i;
                        break;
                    }
                }
                divider.SetSiblingIndex(newSiblingIndex);
            }

        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag (UnifiedDraggableItem)");

        if (copy != null)
        {
            // Handle the end drag logic for the original item
            if (copy.transform.position.x > Screen.width * 0.3f)
            {
                Destroy(copy);
                if (!isPaletteItem)
                {
                    Destroy(gameObject);
                }
                return;
            }

            int newSiblingIndex = parentAfterDrag.childCount;
            for (int i = 0; i < parentAfterDrag.childCount; i++)
            {
                if (copy.transform.position.y > parentAfterDrag.GetChild(i).position.y)
                {
                    newSiblingIndex = i;
                    break;
                }
            }
            copy.transform.SetParent(parentAfterDrag);
            copy.transform.SetSiblingIndex(newSiblingIndex);

            // Hide the divider
            if (divider != null)
            {
                divider.gameObject.SetActive(false);
            }

            if (!isPaletteItem)
            {
                Destroy(gameObject);
            }
            // if (text != null)
            //     text.raycastTarget = true;
        }
    }
}