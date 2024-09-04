using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using Singular;
using UnityEngine;
using UnityEngine.UI;

public class AmbientButton : MonoBehaviour
{
    private AppManager _appManager;
    [SerializeField] public AudioClip buttonClip;
    [SerializeField] private GameObject SelectorIndicator;
    [SerializeField] private int ambientBtnID;
    
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
        if (_appManager.masteryLevel >= 3)
        {
            if (ambientBtnID == 3)
            {
                GetComponent<Button>().interactable = true;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
                
        }
        if (_appManager.masteryLevel >= 5)
        {
            if (ambientBtnID == 4)
            {
                GetComponent<Button>().interactable = true;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnButtonClick2()
    {
        if (_appManager.coins >= 1000)
        {
            GameAnalytics.NewDesignEvent("ButtonClicked:Ambient:" + ambientBtnID);
            GameAnalytics.NewDesignEvent("ButtonClicked2:Ambient", ambientBtnID);
            Dictionary<string, object> buttonClickEvent = new Dictionary<string, object>();
            buttonClickEvent.Add("ButtonType", "Ambient");
            buttonClickEvent.Add("ButtonID", ambientBtnID);
            GameAnalytics.NewDesignEvent("ButtonClicked3",buttonClickEvent);
            _appManager.ClearIndicators();
            SelectorIndicator.SetActive(true);
            _appManager.buttonsPlayer.clip = buttonClip;
            _appManager.buttonsPlayer.Play();
            _appManager.currentAmbient = buttonClip;
            _appManager.ambientButtonClicksCount++;
            _appManager.coins -= 1000;
            PlayerPrefs.SetInt("coins",_appManager.coins);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Coins", 1000, "Coins", "00");
        }
            
        else
        {
            _appManager.coins = 0;
            PlayerPrefs.SetInt("coins",_appManager.coins);
        }
        SingularSDK.Event("AmbientClipChosen", "ClipName", buttonClip.name, "ACC_InSession", _appManager.ambientButtonClicksCount);
    }
}
