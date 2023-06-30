using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SheepNpcVisual : MonoBehaviour
{
    private Tween tween;
    void Start()
    {
        tween = transform.DOLocalMoveY(0.5f , 2f, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(UnityEngine.Random.Range(0.2f, 0.5f))
            .SetEase(Ease.InOutSine);
    }

}
