using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    const string kHasSave      = "HasSave";
    const string kSavedScore   = "SavedScore";
    const string kSavedLives   = "SavedLives";
    const string kHasGameOver  = "HasGameOver";
    const string kGameOverScore= "GameOverScore";

    public static bool HasSave => PlayerPrefs.GetInt(kHasSave, 0) == 1;

    // !! LƯU cả score & lives để resume đúng trạng thái
    public static void SaveState(int score, int lives)
    {
        PlayerPrefs.SetInt(kHasSave, 1);
        PlayerPrefs.SetInt(kSavedScore, Mathf.Max(0, score));
        PlayerPrefs.SetInt(kSavedLives, Mathf.Max(0, lives));
        PlayerPrefs.SetFloat("Volume", 
        AudioManagerScript.instance != null 
        ? AudioManagerScript
            .instance
                .bgmSource
                    .volume
        : 0.5f);
        PlayerPrefs.Save();
    }
    // !! API lưu điểm (giữ lại nếu nơi khác còn gọi)
    public static void Save(int score)
    {
        PlayerPrefs.SetInt(kHasSave, 1);
        PlayerPrefs.SetInt(kSavedScore, Mathf.Max(0, score));
        PlayerPrefs.Save();
    }
    // !! LOAD điểm và lives
    public static int LoadScore() => 
        PlayerPrefs.GetInt(kSavedScore, 0);
    public static int LoadLives() => 
        PlayerPrefs.GetInt(kSavedLives, 5);
    // !! LOAD volume
    public static float LoadVolume() => 
        PlayerPrefs.GetFloat("Volume", 0.5f);
    // !! BẮT ĐẦU GAME MỚI (xóa save cũ)
    public static void NewGame()
    {
        PlayerPrefs.DeleteKey(kHasSave);
        PlayerPrefs.DeleteKey(kSavedScore);
        PlayerPrefs.DeleteKey(kSavedLives);
        PlayerPrefs.Save();
    }
    // !! TIÊU HỦY SAVE KHI ĐÃ RESUME
    public static void ConsumeSave()
    {
        PlayerPrefs.DeleteKey(kHasSave); 
        // ẩn nút Resume cho lần sau
        PlayerPrefs.Save();
    }

    // !! GAME OVER SCORE (bạn đã dùng ở MainMenuUI)
    public static void SaveGameOverScore(int score)
    {
        PlayerPrefs.SetInt(kHasGameOver, 1);
        PlayerPrefs.SetInt(kGameOverScore, Mathf.Max(0, score));
        PlayerPrefs.Save();
    }
    public static bool HasGameOver => 
        PlayerPrefs.GetInt(kHasGameOver, 0) == 1;
    public static int LoadGameOverScore() => 
        PlayerPrefs.GetInt(kGameOverScore, 0);
    // !! XÓA GAME OVER SCORE
    public static void ClearGameOver()
    {
        PlayerPrefs.DeleteKey(kHasGameOver);
        PlayerPrefs.DeleteKey(kGameOverScore);
        PlayerPrefs.Save();
    }
}
