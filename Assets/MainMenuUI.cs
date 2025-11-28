using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// !! MainMenuManager Object
public class MainMenuUI : MonoBehaviour
{
    // ** 25. Giữ lại vị trí trái cây qua các màn chơi SEGMENT
    public static MainMenuUI Instance;

    [Header("Root")]
    public GameObject mainMenuRoot;

    // ** END SEGMENT

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
    // Thêm nhạc nền vào Main Menu?
    // public AudioSource mainMenuBGM;
    // ** END SEGMENT

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        // ** 11. FULLSCREEN FIX SEGMENT
        // Screen.SetResolution(1920, 1080, false);
        // ** END SEGMENT

        // ** 03. MAIN MENU BGM MUSIC SEGMENT
        // Thêm nhạc nền vào Main Menu?
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

        // ** 25. Giữ lại vị trí trái cây qua các màn chơi SEGMENT
        Time.timeScale = 0f;
        // ** END SEGMENT
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
        // ** 24. TẠO LOADING SCENE GIỮA SCENE (DEFAULT)
        SceneManager.LoadScene
        (
            gameplaySceneName, 
            LoadSceneMode.Single
        ); // ** 25. DEFAULT LOAD SCENE
        // StartCoroutine(EnterGameplay()); // ** 25. ENHANCED LOAD SCENE
        // ** END SEGMENT

        // ** 24. TẠO LOADING SCENE GIỮA SCENE (ENHANCED)
        // SceneLoader.LoadSceneWithLoading(gameplaySceneName);
        // ** END SEGMENT

        // ** 02. ENHANCED PAUSE MUSIC SEGMENT
        // TIEP TỤC NHẠC KHI BẮT ĐẦU CHƠI
        // AudioManagerScript
        //     .instance
        //         .ResumeMusic();
        // ** END SEGMENT

        // ** 03. MAIN MENU BGM MUSIC SEGMENT
        // Thêm nhạc nền vào Main Menu?
        // mainMenuBGM.Pause();
        // ** END SEGMENT

    }
    // !! NHẤN NÚT RESUME MAIN MENU
    public void OnResumeClicked()
    {
        // ** 24. TẠO LOADING SCENE GIỮA SCENE (DEFAULT)
        SceneManager.LoadScene(gameplaySceneName); // ** 25. DEFAULT LOAD SCENE
        // StartCoroutine(EnterGameplay()); // ** 25. ENHANCED LOAD SCENE
        // ** END SEGMENT

        // ** 24. TẠO LOADING SCENE GIỮA SCENE (ENHANCED)
        // SceneLoader.LoadSceneWithLoading(gameplaySceneName);
        // ** END SEGMENT

        // ** 02. ENHANCED PAUSE MUSIC SEGMENT
        // AudioManagerScript
        //     .instance
        //         .ResumeMusic();
        // ** END SEGMENT

        // ** 03. MAIN MENU BGM MUSIC SEGMENT
        // Thêm nhạc nền vào Main Menu?
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
    // !! ENTER GAMEPLAY COROUTINE
    System.Collections.IEnumerator EnterGameplay()
    {
        // Ẩn menu
        if (mainMenuRoot) mainMenuRoot.SetActive(false);
        if (creditsPanel) creditsPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        // Nếu GameplayScene chưa load thì load ADDITIVE
        var scene = SceneManager.GetSceneByName(gameplaySceneName);
        if (!scene.isLoaded)
        {
            var op = SceneManager.LoadSceneAsync(
                gameplaySceneName,
                LoadSceneMode.Additive
            );
            while (!op.isDone)
                yield return null;
        }

        // Đặt GameplayScene làm active (cho Instantiate, v.v.)
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameplaySceneName));

        // Tiếp tục thời gian
        Time.timeScale = 1f;
    }

    // !! SHOW MAIN MENU FROM GAMEPLAY FUNCTION
    public void ShowMainMenuFromGameplay()
    {
        if (mainMenuRoot) mainMenuRoot.SetActive(true);
        if (creditsPanel) creditsPanel.SetActive(false);

        // Đang chơi dở → chắc chắn Resume được
        if (resumeButton) resumeButton.SetActive(true);

        if (gameOverPanel) gameOverPanel.SetActive(false);

        RebuildLayoutNow();
        Time.timeScale = 0f;
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