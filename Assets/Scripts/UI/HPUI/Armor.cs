using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Armor : MonoBehaviour
{
    [Header("护甲数据来源")]
    [SerializeField] private Individual individual; // 拖入或自动获取

    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI arText;

    private int currentAR;

    private void Awake()
    {
        currentAR = 0;
    }
    private void Update()
    {
        RefreshFromIndividual();
    }

    private void RefreshFromIndividual()
    {
        if (individual == null) return;
        if (currentAR == individual.Shield_) return; // 无变化则不更新
        currentAR = individual.Shield_;
        // 更新文本
        if (arText != null)
            arText.text = $"{currentAR}";
    }
}
