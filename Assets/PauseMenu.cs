using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider volumeSlider;

    private bool isPaused = false;
    // !! START() PAUSE MENU FUNCTION
    void Start()
    {
        pausePanel.SetActive(false);
        if (AudioManagerScript.instance != null)
        {
            // ** DEFAULT MUSIC
            // Đổi âm lượng trên giao diện theo giá trị của volume hiện tại
            volumeSlider.value = 
                AudioManagerScript
                    .instance
                        .GetMusicVolume();
            // ** END SEGMENT
        }
        volumeSlider
            .onValueChanged
                .AddListener(OnVolumeChanged);
        Time.timeScale = 1f;
    }
    // !! UPDATE() PAUSE MENU FUNCTION
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    // !! THAY ĐỔI GIÁ TRỊ PAUSE
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
    // !! KHI VOLUME THAY ĐỔI 
    void OnVolumeChanged(float v)
    {
        if (AudioManagerScript.instance != null)
        {
            AudioManagerScript
                .instance
                    .SetMusicVolume(v);
        }
    }
    // !! RESUME GAME
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    // !! QUIT TO MAIN MENU
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        int curScore = 
            ScoreManager.Instance 
            ? ScoreManager.Instance.CurrentScore 
            : 0;
        int curLives = 
            PlayerLives.Instance 
            ? PlayerLives.Instance.currentLives 
            : 0;
        SaveSystem.SaveState(curScore, curLives);
        UnityEngine
            .SceneManagement
                .SceneManager
                    .LoadScene
        (
            "MainMenuScene",
            UnityEngine.SceneManagement.LoadSceneMode.Single
        );
    }
}
