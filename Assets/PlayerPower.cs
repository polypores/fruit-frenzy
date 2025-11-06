using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class PlayerPower : MonoBehaviour
{
    [Header("Input & Cooldown")]
    public KeyCode key1 = KeyCode.Return;        // Enter
    public KeyCode key2 = KeyCode.KeypadEnter;   // Enter keypad
    public float cooldown = 3f;

    [Header("FX")]
    public ParticleSystem auraBurstPrefab;       // kéo prefab Particle vào đây

    [Header("Detect Stars")]
    public string starLayerName = "Star";        // tạo Layer "Star" và gán cho prefab Star
    public float gizmoPadding = 0.0f;            // thêm biên nếu muốn

    float nextReadyTime = 0f;

    void Update()
    {
        if ((Input.GetKeyDown(key1) || Input.GetKeyDown(key2)) && Time.time >= nextReadyTime)
        {
            nextReadyTime = Time.time + cooldown;
            Activate();
        }
    }

    void Activate()
    {
        PlayAura();
        ClearStarsInView();
    }

    void PlayAura()
    {
        if (!auraBurstPrefab) return;
        var ps = Instantiate(auraBurstPrefab, transform.position, Quaternion.identity);
        var main = ps.main;
        // Tự hủy sau khi phát xong (1-shot)
        float life = main.duration + main.startLifetime.constantMax + 0.25f;
        Destroy(ps.gameObject, life);
    }

    void ClearStarsInView()
    {
        var cam = Camera.main;
        if (!cam) return;

        float halfH = cam.orthographicSize + gizmoPadding;
        float halfW = halfH * cam.aspect + gizmoPadding;

        // Lấy mọi collider của Layer "Star" trong khung camera
        int mask = LayerMask.GetMask(starLayerName);
        Vector2 center = cam.transform.position;
        Vector2 size   = new Vector2(halfW * 2f, halfH * 2f);

        var hits = Physics2D.OverlapBoxAll(center, size, 0f, mask);
        foreach (var hit in hits)
        {
            if (hit && hit.TryGetComponent<Star>(out var star))
            {
                StarPool.Instance.ReturnStar(star.gameObject); // trả về pool
            }
        }
    }

    // Debug khung quét
    void OnDrawGizmosSelected()
    {
        var cam = Camera.main;
        if (!cam) return;
        float halfH = cam.orthographicSize + gizmoPadding;
        float halfW = halfH * cam.aspect + gizmoPadding;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(cam.transform.position, new Vector3(halfW*2f, halfH*2f, 0f));
    }
}
