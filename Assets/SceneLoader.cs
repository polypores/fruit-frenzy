using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Tên scene sẽ được load sau màn hình Loading
    public static string NextSceneName = "MainMenuScene";

    // Gọi hàm này ở bất kì đâu muốn load với màn hình Loading
    public static void LoadSceneWithLoading(string targetScene)
    {
        NextSceneName = targetScene;
        SceneManager.LoadScene("LoadingScene");
    }
}
