using UnityEngine;
using System.Collections.Generic;

public class MissilePool : MonoBehaviour
{
    public static MissilePool Instance;
    public Missile missilePrefab;
    public int initial = 10;
    public int max = 40;

    readonly Queue<Missile> q = new();
    int created;

    void Awake() { Instance = this; }

    void Start() { for (int i = 0; i < initial; i++) CreateOne(); }

    Missile CreateOne()
    {
        if (created >= max) return null;
        var m = Instantiate(missilePrefab);
        m.gameObject.SetActive(false);
        q.Enqueue(m);
        created++;
        return m;
    }

    public Missile Get()
    {
        if (q.Count == 0) { for (int i = 0; i < 4; i++) if (CreateOne() == null) break; }
        if (q.Count == 0) return null;
        var m = q.Dequeue();
        m.gameObject.SetActive(true);
        return m;
    }

    public void Return(Missile m)
    {
        m.gameObject.SetActive(false);
        q.Enqueue(m);
    }
}
