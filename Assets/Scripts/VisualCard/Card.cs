using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using DG.Tweening;

public class Card : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("卡牌数据")]
    private CardData cardData;

    [Header("UI组件")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI sequenceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("视觉效果")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float dragAlpha = 0.7f;

    [Header("动画设置")]
    [SerializeField] private float returnDuration = 0.3f;
    [SerializeField] private Ease returnEase = Ease.OutCubic;

    private Tween currentTween;
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
            if (sequenceIndex >= 0)
                sequenceText.text = ToRoman(sequenceIndex + 1); // 罗马数字从1开始
            else
                sequenceText.text = "";
        }
    }
    private string ToRoman(int number)
    {
        if (number < 1) return "";
        if (number > 3999) return number.ToString(); // 超出范围直接显示数字

        var romanNumerals = new[]
        {
        new { Value = 10, Numeral = "X" },
        new { Value = 9, Numeral = "IX" },
        new { Value = 5, Numeral = "V" },
        new { Value = 4, Numeral = "IV" },
        new { Value = 1, Numeral = "I" }
    };

        var result = "";
        foreach (var item in romanNumerals)
        {
            while (number >= item.Value)
            {
                result += item.Numeral;
                number -= item.Value;
            }
        }
        return result;
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

        // 停止当前动画
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            currentTween = null;
        }

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
        // 确保父对象正确
        if (originalParent != null && transform.parent != originalParent)
        {
            transform.SetParent(originalParent);
        }

        // 使用动画回到原位置
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        currentTween = transform.DOLocalMove(originalPosition, returnDuration)
            .SetEase(returnEase)
            .OnComplete(() =>
            {
                transform.localScale = Vector3.one;
                currentTween = null;
            });
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

    void OnDestroy()
    {
        // 清理动画
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }

    public string GetCardName() => cardData != null ? cardData.Name_ : "Unnamed Card";
}