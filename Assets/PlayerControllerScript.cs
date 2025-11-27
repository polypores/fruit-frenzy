using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{

    public static PlayerControllerScript instance;

    private float moveSpeed = 7f;

    private float jumpForce = 8f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private Animator anim;
    // ** 04. SFX VOLUME SEGMENT
    private AudioSource playerSfxAudioSource;

    // !! ĐẶT GIÁ TRỊ volume hiệu ứng ng chơi
    public void SetSFXVolume(float volume)
    {
        if (playerSfxAudioSource != null)
        {
            playerSfxAudioSource.volume = volume;
        }
    }
    // ** END SEGMENT
    // !! AWAKE() PLAYER CONTROLLER FUNCTION
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // phòng khi lỡ có 2 Player
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        anim = GetComponent<Animator>();
        playerSfxAudioSource = GetComponent<AudioSource>();
    }
    // !! START() PLAYER CONTROLLER FUNCTION
    void Start()
    {
        // ** 04. SFX VOLUME SEGMENT
        // khởi tạo giá trị cho SFX volume
        if (playerSfxAudioSource != null)
        {
            playerSfxAudioSource.volume = 0.5f;
        }
        // ** END SEGMENT
    }
    // !! UPDATE() PLAYER CONTROLLER FUNCTION
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
        bool isRunning = Mathf.Abs(x) > 0.01f;
        anim.SetBool("isRunning", isRunning);
        handleFootstepsSound(isRunning);
        ClampPosition();
    }
    // !! GIỚI HẠN VỊ TRÍ NG CHƠI TRONG PHẠM VI CHO PHÉP
    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        transform.position = pos;
    }
    // !! XỬ LÝ ÂM THANH CHẠY BỘ
    private void handleFootstepsSound(bool isRunning)
    {
        if (isRunning && isGrounded)
        {
            if (!playerSfxAudioSource.isPlaying)
            {
                playerSfxAudioSource.Play();
            }
        }
        else
        {
            playerSfxAudioSource.Pause();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnCollsionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
