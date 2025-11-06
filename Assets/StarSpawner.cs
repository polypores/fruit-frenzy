using UnityEngine;
using System.Collections;

public class StarSpawner : MonoBehaviour
{
    public float spawnInterval = 0.8f;  
    public float spawnRangeX = 8f;      
    public Vector2 fallSpeedRange = new Vector2(3f, 6f);

    [Range(0f, 1f)] public float bombChance = 0.25f;
    
    [Header("Fruits")]
    [Range(0f,1f)] public float cherryChance = 0.25f;     // phần trăm trong số lần spawn "trái"
    [Range(0f, 1f)] public float pineappleChance = 0.20f;  // còn lại là Star
    
    [Header("Bomb")]
    public Vector2 bombSpeed = new(3.5f, 5.5f);

    [Header("Heart pickup")]
    public float heartStartDelay = 10f;
    public float heartCooldown = 10f;
    float nextHeartTime;
    private float timer;
    
    void Start()
    {
        StartCoroutine(DifficultyRamp());
        nextHeartTime = Time.time + heartStartDelay; // 10s đầu chưa có tim
        StartCoroutine(HeartLoop());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            bool spawnBomb = Random.value < bombChance;
            if (spawnBomb)
                SpawnBomb();
            else
                SpawnFruitLikeStar();
        }
    }

    void SpawnFruitLikeStar()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float speed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);

        float r = Random.value;
        if (r < cherryChance)
        {
            var go = CherryPool.Instance.GetCherry();
            go.transform.position = new Vector3(x, transform.position.y, 0f);
            go.GetComponent<Cherry>().fallSpeed = speed;
        }
        else if (r < cherryChance + pineappleChance)
        {
            var go = PineapplePool.Instance.GetPineapple();
            go.transform.position = new Vector3(x, transform.position.y, 0f);
            go.GetComponent<Pineapple>().fallSpeed = speed;
        }
        else
        {
            // giữ Star như cũ
            SpawnStar(); // hàm sẵn có của bạn
        }
    }

    IEnumerator HeartLoop()
    {
        while (true)
        {
            if (Time.time >= nextHeartTime)
            {
                SpawnHeart();
                nextHeartTime = Time.time + heartCooldown; // mỗi 10s tối đa 1 lần
            }
            yield return null;
        }
    }

    void SpawnHeart()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        var go = HeartPool.Instance.GetHeart();
        go.transform.position = new Vector3(x, transform.position.y, 0f);
        var heart = go.GetComponent<HeartPickup>();
        heart.fallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
    }

    void SpawnBomb()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        var obj = BombPool.Instance.GetBomb();
        
        if (!obj) return;
        obj.transform.position = new Vector3(x, transform.position.y, 0f);
        
        var bomb = obj.GetComponent<Bomb>();
        bomb.fallSpeed = Random.Range(bombSpeed.x, bombSpeed.y);
    }

    void SpawnStar()
    {
        GameObject starObj = StarPool.Instance.GetStar();

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0f);

        starObj.transform.position = spawnPos;

        Star star = starObj.GetComponent<Star>();
        star.fallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
    }

    IEnumerator DifficultyRamp()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); 
            var spawner = GetComponent<StarSpawner>();
            spawner.spawnInterval = Mathf.Max(0.25f, spawner.spawnInterval - 0.1f);
        }
    }

}
