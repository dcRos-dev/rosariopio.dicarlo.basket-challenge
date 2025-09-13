using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{

    [Header("Ui References")]
    [SerializeField] public GameObject loadingPanel, buttonsContainer;
    
    void Start()
    {
        
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
