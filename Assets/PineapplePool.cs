using UnityEngine;
using System.Collections.Generic;

public class PineapplePool : MonoBehaviour
{
    public static PineapplePool Instance;
    public GameObject pineapplePrefab;
    public int initialSize = 30;

    readonly Queue<GameObject> pool = new();

    void Awake()
    {
        Instance = this; 
    }
    void Start()
    {
        for (int i = 0; i < initialSize; i++)
            Create(); 
    }

    GameObject Create()
    {
        var go = Instantiate(pineapplePrefab);
        go.SetActive(false);
        pool.Enqueue(go);
        return go; 
    }

    public GameObject GetPineapple()
    {
        if (pool.Count == 0) 
            for (int i=0;i<10;i++) 
                Create();
        var go = pool.Dequeue();
        go.SetActive(true); 
        return go;
    }

    public void ReturnPineapple(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go); 
    }
}
