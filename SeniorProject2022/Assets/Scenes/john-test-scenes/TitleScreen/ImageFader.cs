using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    private Image _image;
    [SerializeField] private float _initFadeInDuration;
    public bool fadeOnStart = true;

    private void Awake()
    {
        _image = GetComponent<Image>();
        if(fadeOnStart)
        {
            FadeOutToClear(_initFadeInDuration);
        }
    }

    public void FadeOutToClear(float duration)
    {
        _image.DOColor(Color.clear, duration);
    }

    public void FadeInToBlack(float duration)
    {
        _image.DOColor(Color.black, duration);
    }
}
