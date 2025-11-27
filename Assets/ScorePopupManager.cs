using UnityEngine;
using TMPro;

public class ScorePopupManager : MonoBehaviour
{
    public static ScorePopupManager Instance;

    [Header("Setup")]
    public Canvas canvas;                 // ScoreCanvas
    public GameObject popupPrefab;        // Prefab vừa tạo

    [Header("Effect")]
    public float lifeTime = 1.0f;         // tồn tại 1 giây
    public float moveUpDistance = 50f;    // bay lên 50px
    public Vector3 worldOffset = new Vector3(0, 1.5f, 0f); // lệch lên trên đầu player

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
    /// Gọi hàm này mỗi khi muốn hiện "+5" tại vị trí worldPos
    /// </summary>
    public void Show(string text, Vector3 worldPos, Color color)
    {
        if (canvas == null || popupPrefab == null) return;

        // Chuyển tọa độ world sang màn hình
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos + worldOffset);

        // Tạo 1 popup mới dưới Canvas
        GameObject go = Instantiate(popupPrefab, canvas.transform);
        RectTransform rt = go.GetComponent<RectTransform>();
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();

        if (rt != null) rt.position = screenPos;
        if (tmp != null)
        {
            tmp.text = text;
            tmp.color = color;
        }

        // Chạy coroutine để nó tự bay lên rồi biến mất
        StartCoroutine(PopupRoutine(go, rt, tmp));
    }

    System.Collections.IEnumerator PopupRoutine(GameObject go, RectTransform rt, TextMeshProUGUI tmp)
    {
        float t = 0f;
        Vector3 startPos = rt != null ? rt.position : Vector3.zero;
        Color startColor = tmp != null ? tmp.color : Color.white;

        while (t < lifeTime)
        {
            t += Time.deltaTime;
            float progress = t / lifeTime;

            // bay lên
            if (rt != null)
                rt.position = startPos + Vector3.up * moveUpDistance * progress;

            // mờ dần
            if (tmp != null)
            {
                Color c = startColor;
                c.a = 1f - progress;
                tmp.color = c;
            }

            yield return null;
        }

        Destroy(go); // nếu muốn tối ưu sau này bạn có thể đổi thành pooling
    }
}
