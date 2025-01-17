using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor.Timeline; // Import TextMeshPro namespace

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TMP_Text text; // Use TMP_Text for TextMeshPro
    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        text.raycastTarget = false; // Corrected property name
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("End Drag");

        //detect inbetween which objects in the list the item is dropped and set the item's sibling index
        int newSiblingIndex = parentAfterDrag.childCount;
        for (int i = 0; i < parentAfterDrag.childCount; i++) {
            if (transform.position.y > parentAfterDrag.GetChild(i).position.y) {
                newSiblingIndex = i;
                // if (transform.GetSiblingIndex() < newSiblingIndex) {
                //     newSiblingIndex--;
                // }
                break;
            }
        }
        transform.SetParent(parentAfterDrag);
        transform.SetSiblingIndex(newSiblingIndex);

        text.raycastTarget = true; // Corrected property name
    }

}
