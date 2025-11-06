using UnityEngine;
using System.Collections;

public class ResumeApplier : MonoBehaviour
{
    private IEnumerator Start()
    {
        // đợi chắc chắn ScoreManager đã sẵn sàng
        while (ScoreManager.Instance == null || !ScoreManager.Instance.IsReady)
            yield return null;

        // thêm một nhịp frame để mọi thứ settle
        yield return new WaitForEndOfFrame();

        if (!SaveSystem.HasSave) yield break;

        int saved = SaveSystem.LoadScore();
        Debug.Log($"[ResumeApplier] Load saved score = {saved}");
        PlayerLives.Instance.SetLives(SaveSystem.LoadLives());
        ScoreManager.Instance.SetScore(saved);
        // đặt thẳng điểm
        SaveSystem.ConsumeSave();                
        // ẩn nút Resume cho lần sau
    }
}
