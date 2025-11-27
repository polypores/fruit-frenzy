using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// !! PAUSED MANAGER OBJECT
public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider volumeSlider;

    public Slider sfxSlider;

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
        // ** 04. SFX VOLUME SEGMENT
        sfxSlider
            .onValueChanged
                .AddListener(OnSFXVolumeChanged);
        // ** END SEGMENT
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
            // Âm thanh nền nằm ở AudioManagerScript
            AudioManagerScript
                .instance
                    .SetMusicVolume(v);
        }
    }
    // !! 04. KHI SFX VOLUME THAY ĐỔI
    public void OnSFXVolumeChanged(float v)
    {
        if (PlayerControllerScript.instance != null)
        {
            // Âm thanh hiệu ứng nằm ở Player
            PlayerControllerScript
                .instance
                    .SetSFXVolume(v);
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
        // ** 02. ENHANCED PAUSE MUSIC SEGMENT
        // AudioManagerScript
        //     .instance
        //         .PauseMusic();
        // ** END SEGMENT

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
