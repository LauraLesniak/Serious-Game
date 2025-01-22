using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// A base class that implements the drag interface
public abstract class DraggableUI 
    : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected TMP_Text text;
    protected Transform parentAfterDrag;

    protected virtual void Awake()
    {
        // Common logic: find TMP_Text in children
        text = GetComponentInChildren<TMP_Text>();
        if (text == null)
        {
            Debug.LogError($"{name}: No TMP_Text found in children.");
        }
    }

    // Default implementations (can be empty or partial)
    public virtual void OnBeginDrag(PointerEventData eventData) { }
    public virtual void OnDrag(PointerEventData eventData)      { }
    public virtual void OnEndDrag(PointerEventData eventData)   { }

    // Method to set partial alpha for the CanvasGroup
    protected void SetPartialAlpha(float alpha)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = alpha;
    }
}
