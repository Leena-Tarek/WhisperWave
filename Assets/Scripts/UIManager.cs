using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using GameAnalyticsSDK.Setup;
using Singular;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject StartPanel, ASMRPanel, AmbientPanel, EndPanel, InfoPanel, RewardsBtn, CounterPanel, MiniGameEnd,MiniGamePlay,TipPanel;
    [SerializeField] private TextMeshProUGUI coinsText, masteryLvlText, xpText,CounterText, miniGameScoreText,miniGameXPText;
    
    
    //MiniGame
    [SerializeField] private GameObject[] smashBtns;
    private float miniGameTime;
    private bool isMiniGamePlaying,isCurrentlySpawning;
    private int miniGameCounter;

    private AppManager _appManager;
    // Start is called before the first frame update
    void Start()
    {
        _appManager = AppManager.Instance;
        coinsText.text = _appManager.coins.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        coinsText.text = _appManager.coins.ToString();
        if (isMiniGamePlaying)
            MiniGameLogicPlaying();
      
    }

   

    public void StartExperience()
    {
        if (StartPanel)
        {
            ASMRPanel.SetActive(true);
            
            StartPanel.SetActive(false);
            ASMRPanel.transform.DOLocalMoveX(0, 0.5f).From(Screen.width*2);
           
            InfoPanel.SetActive(true);
        }
    }
    
    public void GoToTracks()
    {
        if (ASMRPanel)
        {
            InfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Choose Ambient Sound";
            ASMRPanel.SetActive(false);
            AmbientPanel.SetActive(true);
            AmbientPanel.transform.DOLocalMoveX(0, 0.5f).From(Screen.width*2);
            _appManager.buttonsPlayer.Stop();
            GameAnalytics.NewDesignEvent("Sound_Clicks_Before_Moving", _appManager.ASMRButtonClicksCount);
        }
    }
    public void GoToEndPanel()
    {
        if (AmbientPanel)
        {
            InfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Enjoy Your Mix";
            AmbientPanel.SetActive(false);
            EndPanel.SetActive(true);
            TipPanel.SetActive(false);
            EndPanel.transform.DOLocalMoveX(0, 0.5f).From(Screen.width*2);
            _appManager.buttonsPlayer.Stop();
            GameAnalytics.NewDesignEvent("Ambient_Clicks_Before_Moving", _appManager.ambientButtonClicksCount);
            if(MiniGamePlay)
                MiniGamePlay.SetActive(true);
            if(MiniGameEnd)
                MiniGameEnd.SetActive(false);
            //Start EndGame
           AddXP(1500);

            if (_appManager.endCounter > 1)
            {
                _appManager.endCounter--;
                PlayerPrefs.SetInt("endCounter",_appManager.endCounter);
                CounterText.text = "After " + _appManager.endCounter + " Plays";
                //SetPlayerRefs
            }
                
            else if (_appManager.endCounter <= 1)
            {
                CounterPanel.SetActive(false);
                RewardsBtn.SetActive(true);
                RewardsBtn.transform.DOScale(1.5f, 1).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }


    public void PlayMix()
    {
       

        if (!_appManager.isPlayingMix)
        {
            if (!_appManager.currentASMR || !_appManager.currentAmbient)
            {
                TipPanel.SetActive(true);
                GameAnalytics.NewErrorEvent(GAErrorSeverity.Warning,"No Music Chosen");
            }
            else
            {
                _appManager.ASMRPlayer.clip = _appManager.currentASMR;
                _appManager.AmbientPlayer.clip = _appManager.currentAmbient;
                _appManager.ASMRPlayer.Play();
                _appManager.AmbientPlayer.Play();
                GameAnalytics.NewDesignEvent("PlayedMix:Sound-"+_appManager.currentASMR.name+":Ambient-"+_appManager.currentAmbient.name);
                SingularSDK.Event("MixPlayed");
            }
               
            
        }
        _appManager.isPlayingMix = !_appManager.isPlayingMix;
    }

    public void RestartExperience()
    {
        if (EndPanel)
        {
            InfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Choose ASMR Sound";
            ASMRPanel.SetActive(true);
            EndPanel.SetActive(false);
            ASMRPanel.transform.DOLocalMoveX(0, 0.5f).From(Screen.width*2);
            GameAnalytics.NewDesignEvent("RestartedExperience");
            InfoPanel.SetActive(true);
            _appManager.ASMRPlayer.Stop();
            _appManager.AmbientPlayer.Stop();
            _appManager.currentAmbient = null;
            _appManager.currentASMR = null;
        }
    }

    public void RewardsBtnClicked()
    {
        RewardsBtn.SetActive(false);
        CounterPanel.SetActive(true);
        CounterText.text = "After 5 Plays";
        _appManager.endCounter = 5;
        AddCoins(10000);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Coins", 10000, "Coins", "00");
        GameAnalytics.NewDesignEvent("RewardsButtonClicked");
    }

    public void StartMiniGame()
    {
        GameAnalytics.NewDesignEvent("MiniGameStarted");
        RestartMiniGame();
        isMiniGamePlaying = true;
        if(MiniGamePlay)
            MiniGamePlay.SetActive(false);
        if(MiniGameEnd)
            MiniGameEnd.SetActive(false);
    }

    private void MiniGameLogicPlaying()
    {
        miniGameTime += Time.deltaTime;
        if (miniGameTime > 5)
        {
            isCurrentlySpawning = false;
            isMiniGamePlaying = false;
           
            MiniGameEnd.SetActive(true);
           
            miniGameScoreText.text = _appManager.miniGameScore.ToString();
            AddCoins(_appManager.miniGameScore);
           
            miniGameXPText.text = _appManager.miniGameXP.ToString();
            AddXP(_appManager.miniGameXP);
            
            GameAnalytics.NewDesignEvent("MiniGameEnded:"+_appManager.miniGameScore+":"+_appManager.miniGameXP);
            GameAnalytics.NewDesignEvent("MiniGameEnded2", _appManager.miniGameXP);
          

        }
        else
        {
            if (!isCurrentlySpawning)
            {
                isCurrentlySpawning = true;
                miniGameCounter++;
                StartCoroutine(MiniGameLoop());
            }
        }
    }

    void RestartMiniGame()
    {
        miniGameTime = 0;
        miniGameCounter = 0;
        _appManager.miniGameScore = 0;
        _appManager.miniGameXP = 0;

        foreach (GameObject child in smashBtns)
        {
            child.SetActive(false);
        }
    }

    IEnumerator MiniGameLoop()
    {
      
        int randomBtnIndex = Random.Range(0, smashBtns.Length);
        smashBtns[randomBtnIndex].gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        isCurrentlySpawning = false;
    }
    void AddXP(int xpToAdd)
    {
        _appManager.xp += xpToAdd;
        Debug.Log(_appManager.xp);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "XP", xpToAdd, "XP", "01");
        switch (_appManager.xp)
        {
            case < 2000 and >= 1000:
                _appManager.masteryLevel = 2;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete, "Level"+_appManager.masteryLevel);
                
                break;
            case < 3000 and >= 2000:
                _appManager.masteryLevel = 3;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete, "Level"+_appManager.masteryLevel);
                
                break;
            case < 4000 and >= 3000:
                _appManager.masteryLevel = 4;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete,"Level"+_appManager.masteryLevel);
                
                break;
            case < 5000 and >= 4000:
                _appManager.masteryLevel = 5;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete,"Level"+_appManager.masteryLevel);
               
                break;
            case < 6000 and >= 5000:
                _appManager.masteryLevel = 6;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete,"Level"+_appManager.masteryLevel);
                
                break;
            case < 7000 and >= 6000:
                _appManager.masteryLevel = 7;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete,"Level"+_appManager.masteryLevel);
               
                break;
            case < 8000 and >= 7000:
                _appManager.masteryLevel = 8;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete, "Level"+_appManager.masteryLevel);
              
                break;
            case < 9000 and >= 8000:
                _appManager.masteryLevel = 9;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete, "Level"+_appManager.masteryLevel);
                break;
            case < 10000 and >= 9000:
                _appManager.masteryLevel = 10;
                GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete, "Level"+_appManager.masteryLevel);
                break;
        }
       
        PlayerPrefs.SetInt("masterLevel",_appManager.masteryLevel);
        int tempXp = _appManager.masteryLevel * 1000 - _appManager.xp;

        masteryLvlText.text = _appManager.masteryLevel.ToString();
        xpText.text = 1000-tempXp + "/1000 XP";
        PlayerPrefs.SetInt("currentXP", 1000-tempXp);
        PlayerPrefs.SetInt("xp", _appManager.xp);
        tempXp = 0;
    }

    void AddCoins(int cointToAdd)
    {
        _appManager.coins += cointToAdd;
        PlayerPrefs.SetInt("coins",_appManager.coins);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Coins", cointToAdd, "Coins", "00");
    }
    
    int mod(int a, int n)
    {
        int result = a % n;
        if ((result<0 && n>0) || (result>0 && n<0)) {
            result += n;
        }
        return result;
    }
}
