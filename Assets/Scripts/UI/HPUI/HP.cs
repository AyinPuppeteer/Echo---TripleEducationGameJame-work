using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HP : MonoBehaviour
{
    [Header("血量数据来源")]
    [SerializeField] private Individual individual; // 拖入或自动获取

    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image hpBar;

    private float barFullWidth;
    private int currentHP;
    private int maxHP;
    private Tween hpBarTween;

    void Awake()
    {
        if (individual != null)
        {
            maxHP = individual.MaxHealth_;
            currentHP = individual.Health_;
        }
        if (hpBar != null)
        barFullWidth = hpBar.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        RefreshFromIndividual();
    }

    /// <summary>
    /// 刷新UI，显示Individual的当前血量
    /// </summary>
    public void RefreshFromIndividual()
    {
        if (individual == null) return;
        if (currentHP == individual.Health_ && maxHP == individual.MaxHealth_) return; // 无变化则不更新
        currentHP = individual.Health_;
        maxHP = individual.MaxHealth_;

        // 更新文本
        if (hpText != null)
            hpText.text = $"{currentHP}/{maxHP}";

        // 更新血条（带动效）
        if (hpBar != null)
        {
            float percent = maxHP > 0 ? (float)currentHP / maxHP : 0f;

            // 停止之前的动画
            if (hpBarTween != null && hpBarTween.IsActive())
                hpBarTween.Kill();

            if (hpBar.type == Image.Type.Filled)
            {
                hpBarTween = hpBar.DOFillAmount(percent, 0.3f).SetEase(Ease.OutCubic);
            }
            else
            {
                var size = hpBar.rectTransform.sizeDelta;
                float targetWidth = barFullWidth * percent;
                hpBarTween = DOTween.To(
                    () => size.x,
                    x => {
                        size.x = x;
                        hpBar.rectTransform.sizeDelta = size;
                    },
                    targetWidth,
                    0.3f
                ).SetEase(Ease.OutCubic);
            }
        }
    }
}
