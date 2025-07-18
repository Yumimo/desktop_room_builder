using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optionally do something on drag start
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        // Move UI element with the pointer, scaled by canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}