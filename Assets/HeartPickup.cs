using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] public float fallSpeed;
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

            // ** 16. TIM BAY LÊN THANH MẠNG UI SEGMENT
            // if (HeartFlyUIManager.Instance != null)
            // {
            //     HeartFlyUIManager
            //         .Instance
            //             .PlayHeartFly(other.transform.position);
            // }
            // ** END SEGMENT

            HeartPool.Instance.ReturnHeart(gameObject);
        }
        else if (other.CompareTag("KillZone"))
        {
            HeartPool.Instance.ReturnHeart(gameObject);
        }
    }
}
