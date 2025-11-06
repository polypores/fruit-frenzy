using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Missile : MonoBehaviour
{
    public float speedY = 6f;   // rơi xuống
    float vx;                   // tốc độ xiên ngang

    Rigidbody2D rb;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    public void Launch(float horizontalSpeed)
    {
        vx = horizontalSpeed;
        if (rb) rb.linearVelocity = new Vector2(vx, -speedY);
    }

    void OnEnable() // bảo đảm khi lấy từ pool ra là có vận tốc
    {
        if (rb) rb.linearVelocity = new Vector2(vx, -speedY);
    }

    void FixedUpdate() // cập nhật theo bước vật lý
    {
        if (rb) rb.linearVelocity = new Vector2(vx, -speedY);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Tag Player PHẢI được gán cho nhân vật
        if (other.CompareTag("Player") || other.GetComponent<PlayerControllerScript>() != null)
        {
            PlayerLives.Instance?.Damage(1);
            MissilePool.Instance.Return(this);
            return;
        }

        if (other.CompareTag("KillZone") || other.CompareTag("GroundHitZone"))
        {
            MissilePool.Instance.Return(this);
        }
    }
}
