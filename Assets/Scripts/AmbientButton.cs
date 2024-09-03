using System.Collections;
using System.Collections.Generic;
using Singular;
using UnityEngine;

public class AmbientButton : MonoBehaviour
{
    private AppManager _appManager;
    [SerializeField] public AudioClip buttonClip;
    [SerializeField] private GameObject SelectorIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        _appManager = AppManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick2()
    {
        _appManager.ClearIndicators();
        SelectorIndicator.SetActive(true);
        _appManager.buttonsPlayer.clip = buttonClip;
        _appManager.buttonsPlayer.Play();
        _appManager.currentAmbient = buttonClip;
        _appManager.ambientButtonClicksCount++;
        SingularSDK.Event("AmbientClipChosen", "ClipName", buttonClip.name, "ACC_InSession", _appManager.ambientButtonClicksCount);
    }
}
