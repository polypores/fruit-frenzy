using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    public AudioSource bgmSource;

    private float musicVolume = 0.5f;

    void Awake() {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);     
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.volume = musicVolume;
            bgmSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = musicVolume;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
}
