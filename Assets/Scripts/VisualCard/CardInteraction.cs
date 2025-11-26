using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CardDisplay cardDisplay;

    // 悬停状态
    private bool isHovered = false;
    private bool isInteractive = true;

    // 动画相关
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private const float HoverScale = 1.1f;
    private const float HoverRaise = 20f;

    private void Start()
    {
        InitializeComponents();
        CacheOriginalTransform();
    }

    private void InitializeComponents()
    {
        if (cardDisplay == null)
        {
            cardDisplay = GetComponent<CardDisplay>();
        }
    }

    private void CacheOriginalTransform()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter Detected");

        if (!CanInteract()) return;

        isHovered = true;

        // 显示Tooltip
        if (DynamicTooltipSystem.Instance != null && cardDisplay != null)
        {
            DynamicTooltipSystem.Instance.ShowTooltip(cardDisplay);
        }

        // 悬停动画
        PlayHoverAnimation();

        // 视觉高亮
        //cardDisplay?.SetHighlighted(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanInteract()) return;

        isHovered = false;

        // 隐藏Tooltip
        if (DynamicTooltipSystem.Instance != null)
        {
            DynamicTooltipSystem.Instance.HideTooltip(cardDisplay);
        }

        // 恢复动画
        PlayResetAnimation();

        // 取消高亮
        //cardDisplay?.SetHighlighted(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanInteract()) return;

        // 点击反馈：轻微缩小
        transform.localScale = originalScale * 0.95f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanInteract()) return;

        // 恢复大小
        if (isHovered)
        {
            transform.localScale = originalScale * HoverScale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    // 当卡牌被拖拽时调用（如果你实现了拖拽功能）
    public void OnBeginDrag()
    {
        if (DynamicTooltipSystem.Instance != null)
        {
            DynamicTooltipSystem.Instance.HideTooltip(cardDisplay);
        }

        // 停止所有动画
        StopAllAnimations();
        //cardDisplay?.SetHighlighted(false);
    }

    private bool CanInteract()
    {
        return isInteractive && cardDisplay != null && cardDisplay.CardData != null;
    }

    private void PlayHoverAnimation()
    {
        StopAllAnimations();

        // 使用 LeanTween 或者 Unity 动画
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, originalScale * HoverScale, 0.2f)
                 .setEase(LeanTweenType.easeOutBack);

        LeanTween.moveY(gameObject, originalPosition.y + HoverRaise, 0.2f)
                 .setEase(LeanTweenType.easeOutQuad);
    }

    private void PlayResetAnimation()
    {
        StopAllAnimations();

        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, originalScale, 0.15f)
                 .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.moveY(gameObject, originalPosition.y, 0.15f)
                 .setEase(LeanTweenType.easeInOutQuad);
    }

    private void StopAllAnimations()
    {
        LeanTween.cancel(gameObject);
    }

    public void SetInteractive(bool interactive)
    {
        isInteractive = interactive;

        if (!interactive)
        {
            if (DynamicTooltipSystem.Instance != null)
            {
                DynamicTooltipSystem.Instance.HideTooltip(cardDisplay);
            }

            StopAllAnimations();
            //cardDisplay?.SetHighlighted(false);
        }
    }
}