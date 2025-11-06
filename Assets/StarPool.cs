using UnityEngine;
using System.Collections.Generic;

public class StarPool : MonoBehaviour
{
    public static StarPool Instance;

    public GameObject starPrefab;
    public int initialSize = 50; // số sao tạo sẵn

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewStar();
        }
    }

    private GameObject CreateNewStar()
    {
        GameObject star = Instantiate(starPrefab);
        star.SetActive(false);
        pool.Enqueue(star);
        return star;
    }

    public GameObject GetStar()
    {
        if (pool.Count == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                CreateNewStar();
            }
        }

        GameObject star = pool.Dequeue();
        star.SetActive(true);
        return star;
    }

    public void ReturnStar(GameObject star)
    {
        star.SetActive(false);
        pool.Enqueue(star);
    }
}
