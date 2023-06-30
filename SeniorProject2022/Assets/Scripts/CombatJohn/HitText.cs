using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class HitText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        text.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), .8f).SetEase(Ease.InQuint);
        transform.DOMove(transform.position + Vector3.up * .8f, .9f);
        Destroy(gameObject, 1.0f);
    }

    public void SetText(int damageDone)
    {
        text.text = damageDone.ToString();
    }
}
