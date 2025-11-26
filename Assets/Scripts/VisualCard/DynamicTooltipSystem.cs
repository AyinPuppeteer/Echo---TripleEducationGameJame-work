using UnityEngine;
using System.Collections;
using TMPro;

public class DynamicTooltipSystem : MonoBehaviour
{
    public static DynamicTooltipSystem Instance { get; private set; }

    [Header("Tooltip设置")]
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private float showDelay = 0.3f;
    [SerializeField] private Vector3 offset = new Vector3(150, 0, 0);

    [Header("动画")]
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.15f;

    private GameObject currentTooltip;
    private CanvasGroup currentCanvasGroup;
    private Coroutine showCoroutine;
    private Coroutine followCoroutine;
    private CardDisplay hoveredCard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTooltip(CardDisplay cardDisplay)
    {
        if (cardDisplay == null || cardDisplay.CardData == null) return;

        hoveredCard = cardDisplay;

        // 停止之前的显示协程
        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
        }

        showCoroutine = StartCoroutine(ShowTooltipCoroutine(cardDisplay.CardData));
    }

    public void HideTooltip(CardDisplay cardDisplay)
    {
        if (hoveredCard == cardDisplay)
        {
            hoveredCard = null;

            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            if (currentTooltip != null)
            {
                StartCoroutine(HideTooltipCoroutine());
            }
        }
    }

    private IEnumerator ShowTooltipCoroutine(CardData cardData)
    {
        yield return new WaitForSeconds(showDelay);

        if (hoveredCard == null) yield break;

        // 销毁现有Tooltip
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
        }

        // 创建新Tooltip
        currentTooltip = Instantiate(tooltipPrefab, transform);
        currentCanvasGroup = currentTooltip.GetComponent<CanvasGroup>();

        // 初始化外观
        if (currentCanvasGroup != null)
        {
            currentCanvasGroup.alpha = 0f;
        }

        // 设置Tooltip内容
        InitializeTooltipContent(currentTooltip, cardData);

        // 开始跟随鼠标和淡入
        if (followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
        }
        followCoroutine = StartCoroutine(FollowCursorCoroutine());
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator HideTooltipCoroutine()
    {
        yield return StartCoroutine(FadeOutCoroutine());

        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
            currentCanvasGroup = null;
        }

        if (followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
            followCoroutine = null;
        }
    }

    private void InitializeTooltipContent(GameObject tooltip, CardData cardData)
    {
        // 获取或添加TooltipContent组件
        TooltipContent tooltipContent = tooltip.GetComponent<TooltipContent>();
        if (tooltipContent == null)
        {
            tooltipContent = tooltip.AddComponent<TooltipContent>();
        }

        tooltipContent.Initialize(cardData);
    }

    private IEnumerator FollowCursorCoroutine()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform tooltipRect = currentTooltip.GetComponent<RectTransform>();

        while (currentTooltip != null)
        {
            if (canvas != null && tooltipRect != null)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 tooltipPosition = mousePos + offset;

                // 边界检测
                tooltipPosition = ClampToScreen(tooltipPosition, tooltipRect, canvas);

                currentTooltip.transform.position = tooltipPosition;
            }
            yield return null;
        }
    }

    private Vector3 ClampToScreen(Vector3 position, RectTransform tooltipRect, Canvas canvas)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3[] corners = new Vector3[4];
            tooltipRect.GetWorldCorners(corners);

            float width = corners[2].x - corners[0].x;

            if (position.x + width > Screen.width)
            {
                position.x = position.x - width - offset.x * 2;
            }
        }

        return position;
    }

    private IEnumerator FadeInCoroutine()
    {
        if (currentCanvasGroup == null) yield break;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime && currentCanvasGroup != null)
        {
            elapsedTime += Time.deltaTime;
            currentCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            yield return null;
        }

        if (currentCanvasGroup != null)
        {
            currentCanvasGroup.alpha = 1f;
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (currentCanvasGroup == null) yield break;

        float elapsedTime = 0f;
        float startAlpha = currentCanvasGroup.alpha;

        while (elapsedTime < fadeOutTime && currentCanvasGroup != null)
        {
            elapsedTime += Time.deltaTime;
            currentCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeOutTime);
            yield return null;
        }
    }

    public void ForceHideAllTooltips()
    {
        hoveredCard = null;

        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
            showCoroutine = null;
        }

        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
            currentCanvasGroup = null;
        }

        if (followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
            followCoroutine = null;
        }
    }
}