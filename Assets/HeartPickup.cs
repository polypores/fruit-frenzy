using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [HideInInspector] public float fallSpeed;
    [SerializeField] float killY = -6f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime, Space.World);
        if (transform.position.y < killY)
            HeartPool.Instance.ReturnHeart(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLives.Instance?.Heal(1);                   // +1 mạng
            HeartPool.Instance.ReturnHeart(gameObject);
        }
        else if (other.CompareTag("KillZone"))
        {
            HeartPool.Instance.ReturnHeart(gameObject);
        }
    }
}
