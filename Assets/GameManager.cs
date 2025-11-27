using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreText;

    public int CurrentScore => score;
    public bool IsReady { get; private set; }

    // khởi tạo điểm mỗi lần chơi
    private int score = 0;
    public void SetScore(int value)           
    {
        score = Mathf.Max(0, value);
        UpdateUI();
        SaveSystem.Save(score); // nạp từ resume xong thì ghi luôn để đồng bộ
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        IsReady = true; 
    }

    public void AddScore(int amount)
    {
        score = Mathf.Max(0, score + amount);   // clamp không cho âm
        UpdateUI();
        SaveSystem.Save(score); // autosave
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
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
