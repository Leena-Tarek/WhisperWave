using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using Singular;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class SoundButton : MonoBehaviour
{
    private AppManager _appManager;
    [SerializeField] public AudioClip buttonClip;
    [SerializeField] private GameObject SelectorIndicator;
    [SerializeField] private int soundBtnID;
    
    // Start is called before the first frame update
    void Start()
    {
        _appManager = AppManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if(_appManager!=null)
            UnlockLevels();
    }

   

    void UnlockLevels()
    {
        if (_appManager.masteryLevel >= 2)
        {
            if (soundBtnID == 3)
            {
                GetComponent<Button>().interactable = true;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
                
        }
        if (_appManager.masteryLevel >= 4)
        {
            if (soundBtnID == 4)
            {
                GetComponent<Button>().interactable = true;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    
    
    public void OnButtonClick()
    {
        if (_appManager.coins >= 1000)
        {
            GameAnalytics.NewDesignEvent("ButtonClicked:Sound:" + soundBtnID);
            GameAnalytics.NewDesignEvent("ButtonClicked2:Sound", soundBtnID);
            Dictionary<string, object> buttonClickEvent = new Dictionary<string, object>();
            buttonClickEvent.Add("ButtonType", "Sound");
            buttonClickEvent.Add("ButtonID", soundBtnID);
            GameAnalytics.NewDesignEvent("ButtonClicked3",buttonClickEvent);

            _appManager.ClearIndicators();
            SelectorIndicator.SetActive(true);
            _appManager.buttonsPlayer.clip = buttonClip;
            _appManager.buttonsPlayer.Play();
            _appManager.currentASMR = buttonClip;
            _appManager.ASMRButtonClicksCount++;
        
            _appManager.coins -= 1000;
            PlayerPrefs.SetInt("coins",_appManager.coins);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Coins", 1000, "Coins", "00");
        }
            
        else
        {
            _appManager.coins = 0;
            PlayerPrefs.SetInt("coins",_appManager.coins);
        }
        
        SingularSDK.Event("ASMRClipChosen", "ClipName", buttonClip.name, "ACC_InSession", _appManager.ASMRButtonClicksCount);
    }
}
