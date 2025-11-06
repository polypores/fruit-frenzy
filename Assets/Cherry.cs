using UnityEngine;

public class Cherry : MonoBehaviour
{
    [HideInInspector] public float fallSpeed;
    [SerializeField] float killY = -6f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime, Space.World);
        if (transform.position.y < killY)
            CherryPool.Instance.ReturnCherry(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(5);          // +5
            CherryPool.Instance.ReturnCherry(gameObject);
        }
        else if (other.CompareTag("KillZone"))
        {
            CherryPool.Instance.ReturnCherry(gameObject);
        }
    }
}
