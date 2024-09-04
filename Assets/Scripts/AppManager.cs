using System.Collections;
using System.Collections.Generic;
using Singular;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; } 
    [HideInInspector]
    public AudioClip currentASMR;
    [HideInInspector]
    public AudioClip currentAmbient;

    [SerializeField] public AudioSource buttonsPlayer,ASMRPlayer,AmbientPlayer;
    [HideInInspector] public bool isPlayingMix;

    private AudioClip currentAppStatus;
    [SerializeField] public GameObject[] selectionIndicators;
    [HideInInspector] public int ambientButtonClicksCount, ASMRButtonClicksCount;


    [HideInInspector] public int coins, xp, masteryLevel = 1,endCounter, miniGameScore,miniGameXP,currentXp;
    
    
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
        
       
    }
    // Start is called before the first frame update
    void Start()
    {
        coins = PlayerPrefs.GetInt("coins",10000);
        endCounter = PlayerPrefs.GetInt("endCounter",5);
        masteryLevel = PlayerPrefs.GetInt("masteryLevel",1);
        xp = PlayerPrefs.GetInt("xp",0);
        currentXp = PlayerPrefs.GetInt("currentXP", 0);
        
        SingularSDK.SetDeviceCustomUserId("CustomUserIdLEENAAA");
        Dictionary<string, object> attributes = new Dictionary<string, object>();

// Add attributes
        attributes["key1"] = "value1";
        attributes["key2"] = "value2";
        attributes["key3"] = "value3";
// Add more attributes as needed

        SingularSDK.Event(attributes, "OmarEventName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearIndicators()
    {
        foreach (GameObject child in selectionIndicators)
        {
            child.SetActive(false);
        }
    }
}
