using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Card : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("卡牌数据")]
    private CardData cardData;
    public CardData CardData { get => cardData; }

    [Header("UI组件")]
    [LabelText("卡图")]
    [SerializeField] private Image cardImage;
    [LabelText("卡框")]
    [SerializeField] private Image cardFrame;
    [LabelText("卡背")]
    [SerializeField] private Image cardBack; 
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CardDescription cardDescription;

    [Header("视觉效果")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float dragAlpha = 0.7f;
    [SerializeField] private float FlipDuration = 0.4f;

    [Header("动画设置")]
    [SerializeField] private float returnDuration = 0.3f;
    [SerializeField] private Ease returnEase = Ease.OutCubic;

    private Tween currentTween;
    // 状态
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Transform originalParent;
    private bool isInteractable = true; //是否可交互
    private bool IsBackOver;//是否反面朝上

    private int Index;//序列号（玩家卡牌为0~9，镜像为10~19）
    public int Index_ { get => Index; set => Index = value; }

    public bool IsInteractable => isInteractable; // 接口实现

    private void Start()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardAppearance();
    }

    public void Initialize(CardData data)
    {
        cardData = data;
        data.VisualCard_ = this;
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
            canvasGroup.interactable = interactable;
        }
    }

    private void UpdateCardAppearance()
    {
        if (cardData == null) return;

        if (cardImage != null && cardData.Image_ != null)
        {
            cardImage.sprite = cardData.Image_;
        }
    }

    #region 鼠标交互
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable || isDragging) return;
        transform.localScale = Vector3.one * hoverScale;
        
        cardDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable || isDragging) return;
        transform.localScale = Vector3.one;

        cardDescription.SetActive(false);
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
    #endregion

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

    public void Play(Individual Player)
    {
        if (cardData.Type_ == CardType.攻击) cardData.WhenPlay(Player, Player.Enemy_);
        else cardData.WhenPlay(Player, Player);

        FlipCard(2);
    }

    #region 动画效果
    public void PlayAppearAnimation()
    {
        // 初始状态
        transform.localScale = Vector3.one * 0.1f;
        if (canvasGroup != null) canvasGroup.alpha = 0f;

        // 动画：缩放和透明度
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));
        if (canvasGroup != null) seq.Join(canvasGroup.DOFade(1f, 0.3f));
    }
    public void PlayDisappearAnimation()
    {
        // 动画：缩放和透明度
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack));
        if (canvasGroup != null) seq.Join(canvasGroup.DOFade(0f, 0.3f));
    }

    public void PlayRippleEffect(int rippleLayerCount = 3)
    {
        // 父对象：每次都新建，允许多个同时存在
        string rippleRootName = "RippleEffectRoot";
        GameObject rippleRoot = new GameObject(rippleRootName, typeof(RectTransform));
        rippleRoot.transform.SetParent(transform, false);
        rippleRoot.transform.SetAsFirstSibling();

        RectTransform rootRect = rippleRoot.GetComponent<RectTransform>();
        rootRect.anchorMin = Vector2.one * 0.5f;
        rootRect.anchorMax = Vector2.one * 0.5f;
        rootRect.anchoredPosition = Vector2.zero;
        rootRect.sizeDelta = Vector2.zero;

        float baseDuration = 0.6f;
        float baseScale = 1.0f; // 控制最大扩散比例
        float baseAlpha = 0.6f; // 更明显
        float baseSize = 200f;
        float delayStep = 0.08f;
        float scaleStep = 0.15f; // 层间距更大

        for (int i = 0; i < rippleLayerCount; i++)
        {
            GameObject ripple = new GameObject("RippleLayer", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            ripple.transform.SetParent(rippleRoot.transform, false);

            RectTransform rippleRect = ripple.GetComponent<RectTransform>();
            rippleRect.sizeDelta = new Vector2(baseSize, baseSize) * (1f + i * scaleStep);

            Image rippleImage = ripple.GetComponent<Image>();
            rippleImage.color = new Color(1, 1, 1, baseAlpha / (1f + i * 0.5f)); // 递减更慢
            rippleImage.raycastTarget = false;
            // 可替换为你的圆形Sprite
            // rippleImage.sprite = Resources.Load<Sprite>("你的波纹图片路径");

            ripple.transform.localScale = Vector3.zero;

            float delay = i * delayStep;
            float duration = baseDuration + i * 0.1f;
            float targetScale = baseScale + i * scaleStep;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delay);
            seq.Append(ripple.transform.DOScale(targetScale, duration).SetEase(Ease.OutCubic));
            seq.Join(rippleImage.DOFade(0f, duration));
            seq.OnComplete(() => Destroy(ripple));

            // 每层动画结束后立即销毁
            Destroy(ripple, delay + duration + 0.1f);
        }

        // 父对象销毁时机：最后一层动画结束后再销毁
        float totalDuration = (rippleLayerCount - 1) * delayStep + baseDuration + (rippleLayerCount - 1) * 0.1f + 0.2f;
        Destroy(rippleRoot, totalDuration);
    }
    /// <summary>
    /// 卡片翻转：根据参数决定正面或背面
    /// </summary>
    /// <param name="fliptimes">翻转次数</param>
    public void FlipCard(int fliptimes = 1)
    {
        Sequence seq = DOTween.Sequence();
        for (int i = 1; i <= fliptimes; i++)
        {
            seq.Append(transform.DOLocalRotate(new Vector3(0, 90f, 0), FlipDuration / 2).SetEase(Ease.InCubic))
           .AppendCallback(() =>
           {
               if (IsBackOver)
               {
                   IsBackOver = false;
                   if (cardBack != null) cardBack.enabled = false;
                   if (cardImage != null) cardImage.enabled = true;
                   if (cardFrame != null) cardFrame.enabled = true;
               }
               else
               {
                   IsBackOver = true;
                   if (cardBack != null) cardBack.enabled = true;
                   if (cardImage != null) cardImage.enabled = false;
                   if (cardFrame != null) cardFrame.enabled = false;
               }
           }).Append(transform.DOLocalRotate(new Vector3(0, 0f, 0), FlipDuration / 2).SetEase(Ease.OutCubic));
        }
    }
    #endregion

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