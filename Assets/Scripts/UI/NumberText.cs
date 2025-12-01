using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NumberText : MonoBehaviour
{
    private Text Number;

    private void Awake()
    {
        Number = GetComponent<Text>();
    }

    public void Init(string s, Color c)
    {
        Number.text = s;
        Number.color = c;
    }

    private void Start()
    {
        transform.DOMoveY(transform.position.y + 1f, 0.5f).OnComplete(() => Destroy(gameObject));
    }
}