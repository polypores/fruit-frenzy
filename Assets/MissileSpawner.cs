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
    // ** 08. WAVE SPAWN SETTINGS SEGMENT (CẦU GAI SINH 3 QUẢ MỘT LÚC)
    [Header("Wave Settings")]
    public int missilesPerWave = 3;   // 3 quả / lượt
    public float waveSpacing = 1.5f;  // khoảng cách ngang giữa các quả

    public Transform playerTransform;
    // !! START() MISSILE SPAWNER FUNCTION
    void Start()
    {
        if (!cam) cam = Camera.main;

        if (playerTransform == null && PlayerControllerScript.instance != null)
        {
            playerTransform = PlayerControllerScript.instance.transform;
        }
        StartCoroutine(Loop());
    }
    
    // !! COUTINE LOOP FUNCTION
    IEnumerator Loop()
    {
        yield return new WaitForSeconds(startDelay);
        float t0 = Time.time;
        // Nếu bạn tạm dừng game (Time.timeScale = 0), Time.time sẽ dừng lại (không tăng thêm).
        // Nếu bạn làm chậm game (Time.timeScale = 0.5), Time.time sẽ trôi chậm đi một nửa so với thời gian thực.
        while (true)
        {
            // Nếu đang quá nhiều missile → đợi
            while (alive >= maxConcurrent) yield return null;
            
            float prog = Mathf.Clamp01((Time.time - t0) / rampDuration);
            float interval = Mathf.Lerp(startInterval, minInterval, prog);
            // result = A + (B - A) * t với t ∈ [0, 1]
            // A là startInterval, B là minInterval
            
            // ** 08. CẦU GAI SINH 3 QUẢ MỘT LÚC SEGMENT
            // Làm cho 2 - 3 cầu gai rơi 1 lượt?
            yield return StartCoroutine(SpawnOneWithWarning());
            // yield return StartCoroutine(SpawnWaveWithWarning());
            // ** END SEGMENT

            yield return new WaitForSeconds(interval);
        }
    }
    // !! COUTINE SPAWN ONE WITH WARNING FUNCTION
    IEnumerator SpawnOneWithWarning()
    {
        // chọn x ngẫu nhiên phủ kín chiều ngang camera
        float halfH = cam.orthographicSize;
        // cam.orthographicSize là nửa chiều cao của view trong world units
        float halfW = halfH * cam.aspect;
        // lấy tỷ lệ khung hình để tính nửa chiều rộng từ nửa chiều cao
        float x = Random.Range(cam.transform.position.x - halfW + 0.5f,
                               cam.transform.position.x + halfW - 0.5f);
        // 0.5f để tránh sát mép quá
        // sinh tọa độ x ngẫu nhiên trong khoảng trên
        float y = cam.transform.position.y + halfH + spawnYOffset;
        // cam.transform.position.y + halfH là đỉnh camera
        // + spawnYOffset để cao hơn đỉnh camera một chút

        // biển báo nhỏ ngay mép trên
        Transform warn = null;
        if (warnIconPrefab)
            warn = Instantiate(
                warnIconPrefab, // lấy warnIconPrefab
                new Vector3(
                    x, // tọa độ x đã chọn ở trên
                    cam.transform.position.y + halfH - 0.3f, 
                    // tọa độ y: sát đỉnh camera
                    0f // tọa độ z = 0
                ), 
                Quaternion.identity // hướng xoay
                );

        yield return new WaitForSeconds(warnTime);

        if (warn) Destroy(warn.gameObject);

        // lấy missile từ pool
        var m = MissilePool.Instance.Get();
        if (m == null) yield break;

        // ** 06. DEFAULT TRANSFORM CODE FOR MISSILE SPAWNER SEGMENT

        // m.transform.position = new Vector3(x, y, 0f);
        // m.speedY = speedY;

        // // chọn hướng xiên: vx ∈ [-maxH, -minH] U [minH, maxH]
        // // giá trị vx = 0 thì vật thể rơi thẳng xuống
        // float vx = (Random.value < 0.5f)
        //     ? Random.Range(-maxH, -minH)
        //     : Random.Range(minH,  maxH);
        // m.Launch(vx);

        // ** END SEGMENT

        // ** 06. ENHANCED TRANSFORM CODE FOR MISSILE SPAWNER SEGMENT
        // Cho quả cầu gai hướng về người chơi.
        Vector3 spawnPos = new Vector3(x, y, 0f);
        m.transform.position = spawnPos;
        m.speedY = speedY;

        float vx = 0f;

        if (playerTransform != null)
        {
            // vị trí mục tiêu: chỗ người chơi đứng (có thể dùng player.position.y hoặc y của mặt đất)
            Vector3 targetPos = playerTransform.position;

            // thời gian rơi từ spawnPos.y xuống targetPos.y với tốc độ speedY
            float tFall = (spawnPos.y - targetPos.y) / speedY;

            if (tFall > 0.01f)
            {
                // vx sao cho sau tFall giây x sẽ tới targetPos.x
                vx = (targetPos.x - spawnPos.x) / tFall;

                // giới hạn cho hợp với min/max đã khai báo
                vx = Mathf.Clamp(vx, -maxH, maxH);
                if (Mathf.Abs(vx) < minH)
                    vx = minH * Mathf.Sign(vx);   // tránh quá thẳng đứng nếu muốn
            }
        }
        else
        {
            // fallback: nếu chưa có player thì random như cũ
            vx = (Random.value < 0.5f)
                ? Random.Range(-maxH, -minH)
                : Random.Range(minH,  maxH);
        }

        m.Launch(vx);

        // ** END SEGMENT

        alive++;
        // theo dõi khi “biến mất” để giảm alive
        StartCoroutine(TrackAlive(m));
    }
    
    // !! 08. CẦU GAI SINH 3 QUẢ MỘT LÚC FUNCTION
    IEnumerator SpawnWaveWithWarning()
    {
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        // Chọn vị trí X trung tâm của wave
        float centerX = Random.Range(
            cam.transform.position.x - halfW + 0.5f,
            cam.transform.position.x + halfW - 0.5f
        );

        float ySpawn = cam.transform.position.y + halfH + spawnYOffset;
        float yWarn  = cam.transform.position.y + halfH - 0.3f;

        int count = missilesPerWave;
        Transform[] warns = new Transform[count];

        float leftLimit  = cam.transform.position.x - halfW + 2.5f;
        float rightLimit = cam.transform.position.x + halfW - 2.5f;

        // 1) Tạo biển báo cho cả 3 quả
        for (int i = 0; i < count; i++)
        {
            // i = 0,1,2  → offset = -1, 0, 1 (nếu count=3)
            float offset = (i - (count - 1) * 0.5f) * waveSpacing;
            float x = Mathf.Clamp(centerX + offset, leftLimit, rightLimit);

            if (warnIconPrefab)
            {
                warns[i] = Instantiate(
                    warnIconPrefab,
                    new Vector3(x, yWarn, 0f),
                    Quaternion.identity
                );
            }
        }

        // Đợi cho người chơi thấy dấu chấm than
        yield return new WaitForSeconds(warnTime);

        // Xoá biển báo
        for (int i = 0; i < count; i++)
            if (warns[i]) Destroy(warns[i].gameObject);

        // 2) Spawn 3 quả missile tại các vị trí đã cảnh báo
        for (int i = 0; i < count; i++)
        {
            float offset = (i - (count - 1) * 0.5f) * waveSpacing;
            float x = Mathf.Clamp(centerX + offset, leftLimit, rightLimit);

            var m = MissilePool.Instance.Get();
            if (m == null) continue;

            m.transform.position = new Vector3(x, ySpawn, 0f);
            m.speedY = speedY;

            // Nếu muốn rơi thẳng xuống:
            // float vx = 0f;

            // Hoặc nếu vẫn muốn xiên như code cũ
            float vx = (Random.value < 0.5f)
                ? Random.Range(-maxH, -minH)
                : Random.Range(minH,  maxH);

            m.Launch(vx);

            alive++;
            StartCoroutine(TrackAlive(m));
        }
    }

    IEnumerator TrackAlive(Missile m)
    {
        var go = m.gameObject;
        while (go.activeSelf) yield return null;
        alive = Mathf.Max(0, alive - 1);
    }
}