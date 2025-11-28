using UnityEngine;

public class Cherry : MonoBehaviour
{
    // tốc độ rơi của quả cherry
    [SerializeField] public float fallSpeed;
    [SerializeField] float killY = -6f;
    [SerializeField] float speedMultiplier = 6.0f;
    // !! UPDATE() CHERRY FUNCTION
    void Update()
    {
        // cho vật thể rơi xuống
        transform.Translate
        (
            Vector2.down * fallSpeed * Time.deltaTime * speedMultiplier, 
            Space.World
        );
        // đoạn if này có thể k cần cũng đc, vì đã có OnTriggerEnter2D với KillZone rồi
        if (transform.position.y < killY)
            CherryPool
                .Instance
                    .ReturnCherry(gameObject);
    }
    // !! ONTRIGGERENTER2D() CHERRY FUNCTION
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(5);          // +5

            // ** 19. SEGMENT HIỂN THỊ POPUP ĐIỂM KHI NHẶT CHERRY
            // 🔹 Hiện popup "+5" trên đầu người chơi
            // if (ScorePopupManager.Instance != null)
            // {
            //     ScorePopupManager.Instance.Show(
            //         "+5",
            //         other.transform.position,     // vị trí player
            //         Color.green                   // màu text
            //     );
            // }
            // ** END SEGMENT

            // ** 17. PHÁT TIẾNG TING TING KHI NHẶT CHERRY SEGMENT
            // phát âm thanh nhặt cherry
            // if (PlayerControllerScript.instance != null)
            // {
            //     PlayerControllerScript.instance.PlayCherryPickupSfx();
            // }
            // ** END SEGMENT

            CherryPool.Instance.ReturnCherry(gameObject);
        }
        else if (other.CompareTag("KillZone"))
        {
            CherryPool.Instance.ReturnCherry(gameObject);
        }
    }
}
