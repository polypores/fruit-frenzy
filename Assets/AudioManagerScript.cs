using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    public AudioSource bgmSource;

    // ** ENHANCED MUSIC VOLUME SEGMENT
    // private float musicVolume;
    // ** END SEGMENT
    // ** DEFAULT MUSIC SEGMENT
    private float musicVolume = 0.5f;
    // ** END SEGMENT


    // ! AWAKE() AUDIO MANAGER FUNCTION
    void Awake() {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // ** ENHANCED MUSIC VOLUME: Load musicVolume vào game khi Awake() để lát sau Start() tại PauseMenu có thể lấy đúng giá trị
            // musicVolume = SaveSystem.LoadVolume();
            // instance.SetMusicVolume(musicVolume);
            // ** END SEGMENT
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // ! START() AUDIO MANAGER FUNCTION
    void Start()
    {
        // ** ENDHANCED MUSIC VOLUME SEGMENT
        musicVolume = SaveSystem.LoadVolume();
        // ** END SEGMENT
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;

            //  ** DEFAULT MUSIC SEGMENT
            bgmSource.volume = musicVolume;
            // ** END SEGMENT

            // ** ENHANCED MUSIC SEGMENT
            // bgmSource.volume = SaveSystem.LoadVolume();
            // ** END SEGMENT
            bgmSource.Play();
        }
    }
    // ! ĐẶT GIÁ TRỊ volume HIỆN TẠI (dùng ở PauseMenu)
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = musicVolume;
            // ** ENHANCED MUSIC SEGMENT
            // LƯU LẠI GIÁ TRỊ VÀO PLAYERPREFS
            PlayerPrefs.SetFloat("Volume", Mathf.Max(0, musicVolume));
            // PlayerPrefs.Save();
            // ** END SEGMENT
        }
    }
    // ! LẤY GIÁ TRỊ VOLUME HIỆN TẠI
    public float GetMusicVolume()
    {
        return musicVolume;
    }
}
