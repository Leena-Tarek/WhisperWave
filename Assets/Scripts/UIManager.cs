using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Singular;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject StartPanel, ASMRPanel, AmbientPanel, EndPanel, InfoPanel;

    private AppManager _appManager;
    // Start is called before the first frame update
    void Start()
    {
        _appManager = AppManager.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
    public void GoToEndPanel()
    {
        if (AmbientPanel)
        {
            InfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Enjoy Your Mix";
            AmbientPanel.SetActive(false);
            EndPanel.SetActive(true);
            EndPanel.transform.DOLocalMoveX(0, 0.5f).From(Screen.width*2);
            _appManager.buttonsPlayer.Stop();
        }
    }


    public void PlayMix()
    {
       

        if (!_appManager.isPlayingMix)
        {
            _appManager.ASMRPlayer.clip = _appManager.currentASMR;
            _appManager.AmbientPlayer.clip = _appManager.currentAmbient;
            _appManager.ASMRPlayer.Play();
            _appManager.AmbientPlayer.Play();
            
            SingularSDK.Event("MixPlayed");
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
            InfoPanel.SetActive(true);
            _appManager.ASMRPlayer.Stop();
            _appManager.AmbientPlayer.Stop();
            _appManager.currentAmbient = null;
            _appManager.currentASMR = null;
        }
    }
}
