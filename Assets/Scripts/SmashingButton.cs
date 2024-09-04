using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmashingButton : MonoBehaviour
{
    public AudioClip clip;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.localScale = Vector3.one;
        transform.DOScale(0, 5).From(1).OnComplete(() => gameObject.SetActive(false));
    }

    public void OnSmash()
    {
        AppManager.Instance.miniGameScore += 50;
        AppManager.Instance.miniGameXP += 25;
        audioSource.PlayOneShot(clip);
        gameObject.SetActive(false);
        transform.localScale = Vector3.one;
        
    }
}
