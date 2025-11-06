using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    const string kHasSave      = "HasSave";
    const string kSavedScore   = "SavedScore";
    const string kSavedLives   = "SavedLives";     // <-- thêm
    const string kHasGameOver  = "HasGameOver";
    const string kGameOverScore= "GameOverScore";

    public static bool HasSave => PlayerPrefs.GetInt(kHasSave, 0) == 1;

    // LƯU cả score + lives để Resume đúng trạng thái
    public static void SaveState(int score, int lives)
    {
        PlayerPrefs.SetInt(kHasSave, 1);
        PlayerPrefs.SetInt(kSavedScore, Mathf.Max(0, score));
        PlayerPrefs.SetInt(kSavedLives, Mathf.Max(0, lives));
        PlayerPrefs.Save();
    }

    // API cũ (giữ lại nếu nơi khác còn gọi)
    public static void Save(int score)
    {
        PlayerPrefs.SetInt(kHasSave, 1);
        PlayerPrefs.SetInt(kSavedScore, Mathf.Max(0, score));
        PlayerPrefs.Save();
    }

    public static int LoadScore() => PlayerPrefs.GetInt(kSavedScore, 0);
    public static int LoadLives() => PlayerPrefs.GetInt(kSavedLives, 5);

    public static void NewGame()
    {
        PlayerPrefs.DeleteKey(kHasSave);
        PlayerPrefs.DeleteKey(kSavedScore);
        PlayerPrefs.DeleteKey(kSavedLives);   
        // <-- không còn lỗi undefined
        PlayerPrefs.Save();
    }

    public static void ConsumeSave()
    {
        PlayerPrefs.DeleteKey(kHasSave); 
        // ẩn nút Resume cho lần sau
        PlayerPrefs.Save();
    }

    // Game Over score (bạn đã dùng ở MainMenuUI)
    public static void SaveGameOverScore(int score)
    {
        PlayerPrefs.SetInt(kHasGameOver, 1);
        PlayerPrefs.SetInt(kGameOverScore, Mathf.Max(0, score));
        PlayerPrefs.Save();
    }
    public static bool HasGameOver => PlayerPrefs.GetInt(kHasGameOver, 0) == 1;
    public static int LoadGameOverScore() => PlayerPrefs.GetInt(kGameOverScore, 0);
    public static void ClearGameOver()
    {
        PlayerPrefs.DeleteKey(kHasGameOver);
        PlayerPrefs.DeleteKey(kGameOverScore);
        PlayerPrefs.Save();
    }
}
