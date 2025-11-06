using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundCycler : MonoBehaviour
{
    public Sprite[] backgrounds;  // Kéo các sprite nền vào đây
    public float interval = 10f;  // Đổi mỗi 10 giây
    public bool randomOrder = false;

    SpriteRenderer sr;
    int index = 0;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Background";
        sr.sortingOrder = -100;
    }

    void Start()
    {
        if (backgrounds != null && backgrounds.Length > 0)
        {
            sr.sprite = backgrounds[0];
            FitToCamera();
            StartCoroutine(Cycle());
        }
    }

    void LateUpdate()
    {
        // Nếu camera thay đổi size/aspect khi chơi cửa sổ → refit
        FitToCamera();
    }

    void FitToCamera()
    {
        var cam = Camera.main;
        if (!cam || !sr.sprite) return;

        // Kích thước màn hình theo đơn vị world của camera 2D ortho
        float worldH = cam.orthographicSize * 2f;
        float worldW = worldH * cam.aspect;

        // Kích thước sprite (đơn vị world)
        Vector2 spriteSize = sr.sprite.bounds.size;

        // Scale để sprite phủ kín cả chiều rộng và cao (fit & crop)
        float scaleX = worldW / spriteSize.x;
        float scaleY = worldH / spriteSize.y;
        float scale = Mathf.Max(scaleX, scaleY); // phủ kín

        transform.localScale = new Vector3(scale, scale, 1f);

        // Canh tâm theo camera, luôn đứng sau mọi thứ
        transform.position = new Vector3(cam.transform.position.x,
                                         cam.transform.position.y,
                                         0f);
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (backgrounds == null || backgrounds.Length == 0) yield break;

            if (randomOrder)
                index = Random.Range(0, backgrounds.Length);
            else
                index = (index + 1) % backgrounds.Length;

            sr.sprite = backgrounds[index];
            FitToCamera();
        }
    }
}
