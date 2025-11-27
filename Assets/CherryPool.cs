using UnityEngine;
using System.Collections.Generic;

public class CherryPool : MonoBehaviour
{
    public static CherryPool Instance;
    public GameObject cherryPrefab;
    public int initialSize = 30;

    readonly Queue<GameObject> pool = new();

    void Awake()
    { 
        Instance = this; 
    }
    void Start()
    {
        for(int i=0;i<initialSize;i++) 
        Create(); 
    }

    GameObject Create()
    {
        var go = Instantiate(cherryPrefab); 
        go.SetActive(false); 
        pool.Enqueue(go); 
        return go; 
    }

    public GameObject GetCherry()
    {
        if (pool.Count == 0) 
            for (int i=0; i<10; i++) 
                Create();
        var go = pool.Dequeue(); 
        go.SetActive(true); 
        return go;
    }

    public void ReturnCherry(GameObject go)
    {
        go.SetActive(false); 
        pool.Enqueue(go); 
    }
}
