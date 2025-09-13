using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// this class own the logic of the Main Menu Scene
/// </summary>
public class MainMenuManager : MonoBehaviour 
{

    [Header("UI references")]
    [SerializeField] private GameObject LoadingPanel;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


    /// <summary>
    /// Loads the "Gameplay" scene
    /// </summary>
    public void LoadGameScene()
    {
        //LoadingPanel is used for a decorative element but also as raycast block to prevent multiple interactions by the users.
        if (LoadingPanel != null) LoadingPanel.SetActive(true); 
        SceneManager.LoadSceneAsync("Gameplay"); 
     
    }


}
