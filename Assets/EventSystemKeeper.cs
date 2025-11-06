using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemKeeper : MonoBehaviour
{
    void Awake()
    {
        var existing = FindObjectsOfType<EventSystem>();
        if (existing.Length > 1) { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }
}
