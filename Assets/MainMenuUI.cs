using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// !! MainMenuManager Object
public class MainMenuUI : MonoBehaviour
{

    [Header("Wiring")]
    public GameObject resumeButton;
    public GameObject creditsPanel;
    [Header("Layout")]
    [SerializeField] RectTransform buttonsGroup;
    [Header("Scenes")]
    public string gameplaySceneName = "GameplayScene";
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;     // Panel hiển thị điểm
    public TMP_Text gameOverText;        // "Your score: 123"

    // ** 03. MAIN MENU BGM MUSIC SEGMENT
    // public AudioSource mainMenuBGM;
    // ** END SEGMENT

    void Start()
    {
        // ** 11. FULLSCREEN FIX SEGMENT
        // Screen.SetResolution(1920, 1080, false);
        // ** END SEGMENT

        // ** 03. MAIN MENU BGM MUSIC SEGMENT
        // if (mainMenuBGM != null && !mainMenuBGM.isPlaying)
        // {
        //     mainMenuBGM.loop = true;
        //     mainMenuBGM.volume = 0.5f;
        //     mainMenuBGM.Play();
        // }
        // ** END SEGMENT

        if (creditsPanel) creditsPanel.SetActive(false);
        if (resumeButton) resumeButton.SetActive(SaveSystem.HasSave);
        if (gameOverPanel && SaveSystem.HasGameOver)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText) gameOverText.text =
                "Your Score: " + SaveSystem.LoadGameOverScore();
            SaveSystem.ClearGameOver();
        }
        else if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }
        RebuildLayoutNow();

    }
    void RebuildLayoutNow()
    {
        if (buttonsGroup)
            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsGroup);
    }
    // !! NHẤN NÚT START MAIN MENU
    public void OnStartClicked()
    {
        SaveSystem.NewGame();
        SceneManager.LoadScene
        (
            gameplaySceneName, 
            LoadSceneMode.Single
        );

        // ** 02. ENHANCED PAUSE MUSIC SEGMENT
        // AudioManagerScript
        //     .instance
        //         .ResumeMusic();
        // ** END SEGMENT

        // ** 03. MAIN MENU BGM MUSIC SEGMENT
        // mainMenuBGM.Pause();
        // ** END SEGMENT

    }
    // !! NHẤN NÚT RESUME MAIN MENU
    public void OnResumeClicked()
    {
        SceneManager.LoadScene(gameplaySceneName);

        // ** 02. ENHANCED PAUSE MUSIC SEGMENT
        // AudioManagerScript
        //     .instance
        //         .ResumeMusic();
        // ** END SEGMENT

        // ** 03. MAIN MENU BGM MUSIC SEGMENT
        // mainMenuBGM.Pause();
        // ** END SEGMENT

    }
    // !! HIỂN THỊ PANEL CREDITS
    public void OnShowCredits(bool show)
    {
        if (creditsPanel) 
            creditsPanel.SetActive(show);
    }
    // !! ĐÓNG PANEL GAME OVER
    public void OnCloseGameOverClicked()
    {
        if (gameOverPanel) 
            gameOverPanel.SetActive(false);
    }
    // !! THOÁT GAME MAIN MENU FUNCTION
    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}