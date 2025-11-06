using UnityEngine;
using System.Collections.Generic;

public class BombPool : MonoBehaviour
{
    public static BombPool Instance;

    public GameObject bombPrefab;
    public int initialSize = 20;
    public int maxPoolSize = 100;

    public ParticleSystem defaultExplodeFx;

    private readonly Queue<GameObject> pool = new();
    private int created = 0;

    void Awake() { Instance = this; }

    void Start()
    {
        for (int i = 0; i < initialSize; i++) CreateOne();
    }

    GameObject CreateOne()
    {
        if (created >= maxPoolSize) return null;
        var go = Instantiate(bombPrefab);
        go.SetActive(false);
        pool.Enqueue(go);
        created++;
        return go;
    }

    public GameObject GetBomb()
    {
        if (pool.Count == 0)
        {
            for (int i = 0; i < 5; i++) if (CreateOne() == null) break;
            if (pool.Count == 0) return null;
        }
        var go = pool.Dequeue();
        go.SetActive(true);
        return go;
    }

    public void ReturnBomb(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
