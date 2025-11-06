using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    void Start()
    {
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

    public void OnStartClicked()
    {
        SaveSystem.NewGame();
        SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

    public void OnResumeClicked()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnShowCredits(bool show)
    {
        if (creditsPanel) creditsPanel.SetActive(show);
    }

    public void OnCloseGameOverClicked()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}