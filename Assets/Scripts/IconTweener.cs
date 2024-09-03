using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IconTweener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(1.3f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

   
}
