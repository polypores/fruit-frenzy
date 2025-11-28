using UnityEngine;

public class Pineapple : MonoBehaviour
{
    [SerializeField] public float fallSpeed;
    [SerializeField] float killY = -6f;
    [SerializeField] float speedMultiplier = 1.0f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * speedMultiplier * Time.deltaTime, Space.World);
        if (transform.position.y < killY)
            PineapplePool.Instance.ReturnPineapple(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(8);               // +8
            PineapplePool.Instance.ReturnPineapple(gameObject);
        }
        else if (other.CompareTag("KillZone"))
        {
            PineapplePool.Instance.ReturnPineapple(gameObject);
        }
    }
}