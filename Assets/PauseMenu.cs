using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider volumeSlider;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);

        if (AudioManagerScript.instance != null)
        {
            volumeSlider.value = AudioManagerScript.instance.GetMusicVolume();
        }

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void OnVolumeChanged(float v)
    {
        if (AudioManagerScript.instance != null)
        {
            AudioManagerScript.instance.SetMusicVolume(v);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        int curScore = ScoreManager.Instance ? ScoreManager.Instance.CurrentScore : 0;
        int curLives = PlayerLives.Instance ? PlayerLives.Instance.currentLives : 0;
        SaveSystem.SaveState(curScore, curLives);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
