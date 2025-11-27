using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Bomb : MonoBehaviour
{
    [HideInInspector] public float fallSpeed = 4f;

    [Header("Spin")]
    public float rotateSpeed = 360f; // độ/giây, 360 = quay 1 vòng/giây

    [Header("Explosion")]
    public float explodeRadius = 1.2f;
    public int penaltyPoints = 5;
    public LayerMask playerMask;          // chọn layer của Player
    public AssetReferenceGameObject explodeFxRef;      // kéo prefab FX nổ (1-shot)

    [SerializeField] float killY = -6f;

    bool exploded;

    void OnEnable() { exploded = false; }

    void Update()
    {
        // ** 14. DEFAULT BOMB SPIN SEGMENT
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime, Space.World);

        if (transform.position.y < killY)
            BombPool.Instance.ReturnBomb(gameObject);
        // ** END SEGMENT

        // ** 14. ENHANCED BOMB SPIN SEGMENT
        // transform.Translate(Vector2.down * fallSpeed * Time.deltaTime, Space.World);
        // transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime, Space.Self);

        // if (transform.position.y < killY)
        //     BombPool.Instance.ReturnBomb(gameObject);
        // ** END SEGMENT
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;

        // Chạm người → nổ ngay, chắc chắn trừ điểm
        if (other.CompareTag("Player"))
        {
            Explode(true);
            return;
        }

        // Đến mép mặt đất → nổ
        if (other.CompareTag("GroundHitZone"))
        {
            Explode(false);
            return;
        }

        // Phòng hờ: lọt KillZone (không nên xảy ra nếu GroundHitZone đúng vị trí)
        if (other.CompareTag("KillZone"))
        {
            BombPool.Instance.ReturnBomb(gameObject); 
            // nếu bạn dùng pool riêng cho bom, đổi lại BombPool
        }
    }

    void Explode(bool hitPlayerDirect)
    {
        exploded = true;

        // FX nổ
        // if (explodeFx)
        // {
        //     var fx = Instantiate(explodeFx, transform.position, Quaternion.identity);
        // }

        if (explodeFxRef != null && explodeFxRef.RuntimeKeyIsValid())
        {
            var handle = Addressables.InstantiateAsync(
                explodeFxRef, 
                transform.position, 
                Quaternion.identity
            );
            StartCoroutine(ReleaseFxWhenDone(handle));   
            // đợi FX chạy xong rồi release instance
        }

        // Quét bán kính nổ – nếu Player trong vùng → trừ điểm
        bool damaged = false;
        var hits = Physics2D.OverlapCircleAll(
            transform.position, 
            explodeRadius, 
            playerMask
        );
        foreach (var h in hits)
        {
            if (h.CompareTag("Player"))
            {
                ScoreManager.Instance.AddScore(-penaltyPoints);
                damaged = true;
                break;
            }
        }

        // (Optional) nếu chạm trực tiếp người mà mask không khớp vẫn đảm bảo trừ
        if (hitPlayerDirect && !damaged)
            ScoreManager.Instance.AddScore(-penaltyPoints);

        // Trả về pool
        BombPool.Instance.ReturnBomb(gameObject);
    }

    // Debug bán kính nổ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }


    System.Collections.IEnumerator ReleaseFxWhenDone(AsyncOperationHandle<GameObject> h)
    {
        yield return h;
        
        if (h.Status == AsyncOperationStatus.Succeeded)
        {
            var go = h.Result;
            var ps = go.GetComponent<ParticleSystem>();
            float life = 0.5f;

            if (ps)
            {
                var main = ps.main;
                life = main.duration + main.startLifetime.constantMax + 0.25f;
            }

            yield return new WaitForSeconds(life);
            Addressables.ReleaseInstance(go); // tự hủy & giải phóng
        }
    }
}
