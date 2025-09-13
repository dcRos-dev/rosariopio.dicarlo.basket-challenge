using System;
using TMPro; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [Header("Gameplay settings")]
    [SerializeField] private float maxTime;
   
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject LoadingPanel;




    public static event Action OnTimeFinished;
    private float timer;


    void Start()
    {
        //initializing timer 
        timer = maxTime;


    }

    private void OnEnable()
    {
        OnTimeFinished += LoadResults;
    }

    private void OnDisable()
    {
        OnTimeFinished -= LoadResults;
    }


    void Update()
    {
        if(timer > 0)
        {
            //decreasing the timer by the time speed
            timer -= Time.deltaTime;

            //Updating Timer Ui
            int min = Mathf.FloorToInt(timer / 60f);
            int sec = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"{min:00}:{sec:00}";
        }
        else
        {
            //timed out
            timer = 0;
            OnTimeFinished?.Invoke();
        }
    }


    private void LoadResults()
    {
        OnTimeFinished -= LoadResults; // avoid multiple invocations
        //game ended, shows results
        LoadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync("Reward");
    }

}
