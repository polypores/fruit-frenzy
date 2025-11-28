using UnityEngine;

public class Star : MonoBehaviour
{

    [SerializeField] public float fallSpeed;

    [SerializeField] float killY = -6f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(
            Vector2.down * fallSpeed * Time.deltaTime, 
            Space.World
        );

        if (transform.position.y < killY)
            StarPool.Instance.ReturnStar(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(1);

            StarPool.Instance.ReturnStar(gameObject);
        }
        else if (other.CompareTag("KillZone"))
        {
            StarPool.Instance.ReturnStar(gameObject);
        }
    }
}