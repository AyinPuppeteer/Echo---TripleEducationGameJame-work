using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardInteraction : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("组件引用")]
    [SerializeField] private Image cardImage;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("悬停效果")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float hoverRaise = 20f;

    [Header("拖拽设置")]
    [SerializeField] private float dragAlpha = 0.7f;

    // 状态
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Transform originalParent;
    private CardArea currentArea;

    // 事件
    public System.Action<CardInteraction> OnHoverStart;
    public System.Action<CardInteraction> OnHoverEnd;
    public System.Action<CardInteraction, CardArea> OnCardDropped;

    private void Start()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (cardImage == null) cardImage = GetComponent<Image>();
        FindCurrentArea();
    }

    // 鼠标悬停预览
    public void OnPointerEnter(PointerEventData eventData) => OnHoverStart?.Invoke(this);
    public void OnPointerExit(PointerEventData eventData) => OnHoverEnd?.Invoke(this);

    // 拖拽功能
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalPosition = transform.localPosition;
        originalParent = transform.parent;

        canvasGroup.alpha = dragAlpha;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(GetComponentInParent<Canvas>().transform);

        OnHoverEnd?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform as RectTransform, eventData.position,
            eventData.pressEventCamera, out Vector3 worldPoint))
        {
            transform.position = worldPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        CardArea dropArea = FindDropArea(eventData);
        if (dropArea != null && dropArea != currentArea)
        {
            MoveToArea(dropArea);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private CardArea FindDropArea(PointerEventData eventData)
    {
        foreach (var result in eventData.hovered)
        {
            CardArea area = result.GetComponent<CardArea>();
            if (area != null && area.CanAcceptCard(this)) return area;
        }
        return null;
    }

    private void MoveToArea(CardArea newArea)
    {
        currentArea?.RemoveCard(this);
        newArea.AddCard(this);
        currentArea = newArea;
        OnCardDropped?.Invoke(this, newArea);
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        StartCoroutine(MoveToPosition(originalPosition, 0.3f));
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        float elapsed = 0f;
        Vector3 start = transform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }
        transform.localPosition = target;
    }

    private void FindCurrentArea()
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            CardArea area = parent.GetComponent<CardArea>();
            if (area != null) { currentArea = area; break; }
            parent = parent.parent;
        }
    }

    public void SetCurrentArea(CardArea area) => currentArea = area;
    public CardArea GetCurrentArea() => currentArea;
}