using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class Card : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("卡牌数据")]
    [SerializeField] private CardData cardData;

    [Header("UI组件")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI sequenceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("视觉效果")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float dragAlpha = 0.7f;

    // 状态
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Transform originalParent;
    private int sequenceIndex = -1;
    private bool isInCommandArea = false;
    private bool isInteractable = true; // 新增：交互控制

    public CardData CardData => cardData;
    public bool IsInteractable => isInteractable; // 接口实现

    private void Start()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardAppearance();
        UpdateSequenceDisplay();
    }

    public void Initialize(CardData data)
    {
        cardData = data;
        UpdateCardAppearance();
    }

    // 接口实现：设置交互状态
    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;

        // 更新视觉反馈
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = interactable;
            canvasGroup.alpha = interactable ? 1f : 0.5f;
        }
    }

    private void UpdateCardAppearance()
    {
        if (cardData == null) return;

        if (cardImage != null && cardData.Image_ != null)
        {
            cardImage.sprite = cardData.Image_;
        }

        if (nameText != null) nameText.text = cardData.Name_;
        if (valueText != null) valueText.text = cardData.Value_.ToString();
    }

    // 序列号管理
    public void SetSequenceIndex(int index)
    {
        if (sequenceIndex != index)
        {
            sequenceIndex = index;
            UpdateSequenceDisplay();
        }
    }

    private void UpdateSequenceDisplay()
    {
        if (sequenceText != null)
        {
            sequenceText.text = (sequenceIndex + 1).ToString();
            sequenceText.gameObject.SetActive(sequenceIndex >= 0);
        }
    }

    // 区域状态
    public void SetInCommandArea(bool inCommandArea)
    {
        isInCommandArea = inCommandArea;
        UpdateSequenceDisplay();
    }

    // 鼠标交互
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable || isDragging) return;
        transform.localScale = Vector3.one * hoverScale;
        Debug.Log("OnBeginDrag triggered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable || isDragging) return;
        transform.localScale = Vector3.one;
    }

    // 拖拽功能
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;
        isDragging = true;
        originalPosition = transform.localPosition;
        originalParent = transform.parent;

        canvasGroup.alpha = dragAlpha;
        canvasGroup.blocksRaycasts = false;

        // 提升层级确保在拖拽时显示在最前面
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractable || !isDragging) return;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            transform as RectTransform, eventData.position,
            eventData.pressEventCamera, out Vector3 worldPoint))
        {
            transform.position = worldPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 检测是否在指令区内
        CommandArea commandArea = FindCommandArea(eventData);
        if (commandArea != null)
        {
            commandArea.HandleCardDrop(this, transform.position);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private CommandArea FindCommandArea(PointerEventData eventData)
    {
        foreach (var result in eventData.hovered)
        {
            CommandArea area = result.GetComponent<CommandArea>();
            if (area != null) return area;
        }
        return null;
    }

    public void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        StartCoroutine(MoveToPosition(originalPosition, 0.2f));
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
        transform.localScale = Vector3.one;
    }

    public string GetCardName() => cardData != null ? cardData.Name_ : "Unnamed Card";
}