using UnityEngine;
using System.Collections.Generic;

public class HeartPool : MonoBehaviour
{
    public static HeartPool Instance;
    public GameObject heartPrefab;
    public int initialSize = 10;

    readonly Queue<GameObject> pool = new();

    void Awake(){ Instance = this; }
    void Start(){ for(int i=0;i<initialSize;i++) Create(); }

    GameObject Create(){ var go = Instantiate(heartPrefab); go.SetActive(false); pool.Enqueue(go); return go; }

    public GameObject GetHeart()
    {
        if (pool.Count == 0) for (int i=0;i<10;i++) Create();
        var go = pool.Dequeue(); go.SetActive(true); return go;
    }

    public void ReturnHeart(GameObject go){ go.SetActive(false); pool.Enqueue(go); }
}
