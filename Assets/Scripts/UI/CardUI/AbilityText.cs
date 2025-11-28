using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityText : MonoBehaviour
{
    private TextMeshProUGUI Name;
    private TextMeshProUGUI Text;

    public void SetData(AbilityPack ability)
    {
        Name.text = ability.Name;
        Text.text = ability.Description;
    }
}