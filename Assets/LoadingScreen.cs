using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    // Optional: nếu bạn muốn đổi text theo progress
    public TMP_Text loadingText;

    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    System.Collections.IEnumerator LoadNextScene()
    {
        // Lấy tên scene đã được đặt bởi SceneLoader
        string target = SceneLoader.NextSceneName;
        if (string.IsNullOrEmpty(target))
        {
            // fallback, lỡ ai chạy thẳng LoadingScene
            target = "MainMenuScene";
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(target);
        // Nếu muốn màn hình Loading chắc chắn hiện tối thiểu 0.5s:
        float minTime = 0.5f;
        float t = 0f;

        while (!op.isDone || t < minTime)
        {
            t += Time.deltaTime;

            // Optional: hiển thị phần trăm
            if (loadingText != null)
            {
                float progress = Mathf.Clamp01(op.progress / 0.9f);
                loadingText.text = "Loading... " + Mathf.RoundToInt(progress * 100f) + "%";
            }

            yield return null;
        }
    }
}
