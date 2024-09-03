using System.Collections;
using System.Collections.Generic;
using Singular;
using UnityEngine;

public class SoundButton : MonoBehaviour
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

    public void OnButtonClick()
    {
        _appManager.ClearIndicators();
        SelectorIndicator.SetActive(true);
        _appManager.buttonsPlayer.clip = buttonClip;
        _appManager.buttonsPlayer.Play();
        _appManager.currentASMR = buttonClip;
        _appManager.ASMRButtonClicksCount++;
        SingularSDK.Event("ASMRClipChosen", "ClipName", buttonClip.name, "ACC_InSession", _appManager.ASMRButtonClicksCount);
    }
}
