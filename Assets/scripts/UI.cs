using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace event_types 
{
    public delegate void UIDelegate();
}
public class UI : MonoBehaviour
{
    public static UI Instance;


    [SerializeField] private GameObject _LostUI;
    [SerializeField] private GameObject _WonUI;
    [SerializeField] private TMP_Text _Timer;
    [SerializeField] private int secondsToWin;

   
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateTimer();
    }
    void Init() 
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
    }
    
    public void ShowLostMenu() 
    {
        _LostUI.SetActive(true);
    }

    public void ShowWonUI()
    {
        _WonUI.SetActive(true);
        StartCoroutine("changeSceneAfter", 1);
    }
    IEnumerator changeSceneAfter(int sec) 
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    private void UpdateTimer() 
    {
        int ForwardTime = (int)Time.time;
        
        int BackTime = secondsToWin-ForwardTime;

        
        int mins, secs;
        mins = BackTime / 60;
        secs = BackTime - (mins*60);

        _Timer.text = $"[{mins}:{secs}]";
        
        if (BackTime <= 0)
            {
                ShowWonUI();
            }
    }
}
