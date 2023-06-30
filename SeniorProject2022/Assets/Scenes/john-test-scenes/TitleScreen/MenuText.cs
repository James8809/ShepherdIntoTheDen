using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SuperTextMesh _text;
    private Button _button;
    private string original_text;
    private float fade;
    private Tween scaleTween = null;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<SuperTextMesh>();
        original_text = _text.text;
        fade = _text.fade;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.text = "<j>" + original_text;
        scaleTween?.Kill(true);
        scaleTween = transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), .2f);
        _text.fade = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.text = original_text;
        _text.fade = fade;
        scaleTween?.Kill(true);
        scaleTween = transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), .01f);
    }
}
