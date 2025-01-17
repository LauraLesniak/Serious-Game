using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public void OnDrop(PointerEventData eventData) {
        GameObject dropped = eventData.pointerDrag;
        DragItem dragItem = dropped.GetComponent<DragItem>();
        dragItem.parentAfterDrag = transform;
    }
}
