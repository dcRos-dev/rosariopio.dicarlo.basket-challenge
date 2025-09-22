using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{

    [Header("Ui References")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    void Start()
    {
        scoreText.text = GameData.FinalScore.ToString();
    }

    void Update()
    {
        
    }


    public void LoadScene(string scene)
    {
        buttonsContainer.SetActive(false);
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync(scene);
    }
}
