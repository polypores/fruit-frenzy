using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartFlyUIManager : MonoBehaviour
{
    public static HeartFlyUIManager Instance;

    [Header("Setup")]
    public Canvas canvas;                 // Canvas HUD (ScoreCanvas / UICanvas...)
    public RectTransform heartsTarget;    // UI thanh tim góc trái
    public GameObject flyingHeartPrefab;  // Prefab tim nhỏ (UI Image)

    [Header("Effect")]
    public float flyDuration = 0.7f;      // thời gian bay
    public float startScale = 1f;         // scale lúc mới xuất hiện
    public float endScale = 0.5f;         // scale khi tới thanh tim

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gọi hàm này khi ăn mạng: tim bay từ vị trí world của player lên thanh tim
    /// </summary>
    public void PlayHeartFly(Vector3 worldStartPos)
    {
        if (canvas == null || heartsTarget == null || flyingHeartPrefab == null)
            return;

        Camera cam = Camera.main;
        if (cam == null) return;

        // Chuyển vị trí world của player sang vị trí màn hình
        Vector3 startScreenPos = cam.WorldToScreenPoint(worldStartPos);

        // Tạo 1 tim nhỏ dưới Canvas
        GameObject go = Instantiate(flyingHeartPrefab, canvas.transform);
        RectTransform rt = go.GetComponent<RectTransform>();
        Image img = go.GetComponent<Image>();

        rt.position = startScreenPos;
        rt.localScale = Vector3.one * startScale;

        StartCoroutine(FlyRoutine(rt, img, startScreenPos, heartsTarget.position));
    }

    IEnumerator FlyRoutine(RectTransform rt, Image img,
                           Vector3 startScreenPos, Vector3 targetScreenPos)
    {
        float t = 0f;
        Color startColor = img != null ? img.color : Color.white;

        while (t < flyDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / flyDuration);

            // Smoothstep easing cho đường bay mượt
            float eased = p * p * (3f - 2f * p);

            if (rt != null)
            {
                rt.position = Vector3.Lerp(startScreenPos, targetScreenPos, eased);

                float scale = Mathf.Lerp(startScale, endScale, p);
                rt.localScale = Vector3.one * scale;
            }

            if (img != null)
            {
                Color c = startColor;
                c.a = 1f - p;   // mờ dần
                img.color = c;
            }

            yield return null;
        }

        if (rt != null)
            Destroy(rt.gameObject);
    }
}
