using UnityEngine;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    [Header("Refs")]
    public Transform warnIconPrefab; // 1 sprite nhỏ “!” (world-space)
    public Camera cam;

    [Header("Timing")]
    public float startDelay = 10f;        // sau 10 giây mới bắn
    public float startInterval = 3.0f;    // ban đầu mỗi 3 giây 1 quả
    public float minInterval = 1.0f;      // nhanh nhất là 1 giây/quả
    public float rampDuration = 60f;      // sau ~60s đạt minInterval

    [Header("Limits")]
    public int maxConcurrent = 4;         // không quá nhiều để còn né
    public float warnTime = 0.7f;         // thời gian hiện biển báo
    public float spawnYOffset = 1.0f;     // spawn cao hơn đỉnh camera

    [Header("Missile Move")]
    public float minH = 1.2f;             // tốc độ ngang tối thiểu để thành xiên
    public float maxH = 3.0f;             // tốc độ ngang tối đa
    public float speedY = 6f;             // rơi dọc

    int alive;

    void Start()
    {
        if (!cam) cam = Camera.main;
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        yield return new WaitForSeconds(startDelay);
        float t0 = Time.time;

        while (true)
        {
            // Nếu đang quá nhiều missile → đợi
            while (alive >= maxConcurrent) yield return null;

            float prog = Mathf.Clamp01((Time.time - t0) / rampDuration);
            float interval = Mathf.Lerp(startInterval, minInterval, prog);

            yield return StartCoroutine(SpawnOneWithWarning());

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator SpawnOneWithWarning()
    {
        // chọn x ngẫu nhiên phủ kín chiều ngang camera
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        float x = Random.Range(cam.transform.position.x - halfW + 0.5f,
                               cam.transform.position.x + halfW - 0.5f);
        float y = cam.transform.position.y + halfH + spawnYOffset;

        // biển báo nhỏ ngay mép trên
        Transform warn = null;
        if (warnIconPrefab)
            warn = Instantiate(warnIconPrefab, new Vector3(x, cam.transform.position.y + halfH - 0.3f, 0f), Quaternion.identity);

        yield return new WaitForSeconds(warnTime);

        if (warn) Destroy(warn.gameObject);

        // lấy missile từ pool
        var m = MissilePool.Instance.Get();
        if (m == null) yield break;

        m.transform.position = new Vector3(x, y, 0f);
        m.speedY = speedY;

        // chọn hướng xiên: vx ∈ [-maxH, -minH] U [minH, maxH]
        float vx = (Random.value < 0.5f)
            ? Random.Range(-maxH, -minH)
            : Random.Range(minH,  maxH);
        m.Launch(vx);

        alive++;
        // theo dõi khi “biến mất” để giảm alive
        StartCoroutine(TrackAlive(m));
    }

    IEnumerator TrackAlive(Missile m)
    {
        var go = m.gameObject;
        while (go.activeSelf) yield return null;
        alive = Mathf.Max(0, alive - 1);
    }
}