using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Name;
    [SerializeField]
    private TextMeshProUGUI Text;

    public void SetData(AbilityPack ability)
    {
        Name.text = ability.Name;
        Text.text = ability.Description;
    }
}