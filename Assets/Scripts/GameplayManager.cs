using System;
using TMPro; 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameplayManager : MonoBehaviour
{

    [Header("Scene References")]
    [SerializeReference] private HoopTriggerHandler hoopTrigger;
    [Space]
    [Header("Gameplay settings")]
    [SerializeField] private float maxTime;
    [Space]
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject LoadingPanel;



    public static event Action OnTimeFinished;


    //player score
    private int score { get; set; } = 0;
    
    private float timer;
    private bool timeEnded = false;


    void Start()
    {
        //initializing timer 
        timer = maxTime;
        UpdateScoreUI(score);
    }

    private void OnEnable()
    {
        OnTimeFinished += LoadResults;
        hoopTrigger.OnScore += IncreaseScore;
    }

    private void OnDisable()
    {
        OnTimeFinished -= LoadResults;
        hoopTrigger.OnScore -= IncreaseScore;
    }


    void Update()
    {
        if (!timeEnded && timer > 0)
        {
            timer -= Time.deltaTime;
            int min = Mathf.FloorToInt(timer / 60f);
            int sec = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"{min:00}:{sec:00}";
        }
        else if (!timeEnded)
        {
            timeEnded = true;
            timer = 0;
            OnTimeFinished?.Invoke();
        }
    }


    /// <summary>
    /// Adds the specified number of points to the player's score.
    /// </summary>
    /// <param name="pt">The number of points to add to the score.</param>
    public void IncreaseScore(int pt)
    {
        score += pt;
        UpdateScoreUI(score);
        Debug.Log("Earned points: " + pt);
        Debug.Log("Current score: " + score);
    }



    private void LoadResults()
    {
        OnTimeFinished -= LoadResults; // avoid multiple invocations
        //game ended, shows results
        LoadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync("Reward");
    }

   

    private void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }



}
