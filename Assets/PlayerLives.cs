using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public static PlayerLives Instance;

    [Header("Lives")]
    public int maxLives = 5;
    public int currentLives;

    [Header("UI")]
    public Image heartIcon;
    // Ảnh icon trái tim (UI Image)
    public TMP_Text heartsText;   
    // "x5" v.v.

    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenuScene";

    void Awake() { Instance = this; }

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();
    }
    public void Heal(int amount = 1)
    {
        currentLives = Mathf.Min(maxLives, currentLives + amount);
        UpdateUI();
    }
    public void Damage(int amount = 1)
    {
        currentLives = Mathf.Max(0, currentLives - amount);
        UpdateUI();
        if (currentLives <= 0)
            GameOver();
    }

    void UpdateUI()
    {
        if (heartsText) heartsText.text = "x" + currentLives;
        if (heartIcon) heartIcon.enabled = currentLives > 0;
    }

    void GameOver()
    {
        int cur = ScoreManager.Instance ? ScoreManager.Instance.CurrentScore : 0;
        Debug.Log("[PlayerLives] GameOver save score = " + cur);
        SaveSystem.SaveGameOverScore(cur);
        // để MainMenu hiển thị panel điểm
        SaveSystem.NewGame();
        // xóa HasSave/SavedScore để ẩn Resume
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void SetLives(int value)
    {
        currentLives = Mathf.Clamp(value, 0, maxLives);
        UpdateUI();
    }

}
