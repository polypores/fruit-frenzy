using UnityEngine;

// !! AudioManager Object
public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    public AudioSource bgmSource;

    // ** 01. ENHANCED MUSIC VOLUME SEGMENT - LƯU VOLUME HIỆN TẠI BẰNG PLAYERPREFS
    // private float musicVolume;
    // ** END SEGMENT

    // ** 01. DEFAULT MUSIC SEGMENT
    private float musicVolume = 0.5f;
    // ** END SEGMENT


    // ! AWAKE() AUDIO MANAGER FUNCTION
    void Awake() {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // ** 01. ENHANCED MUSIC SEGMENT: Load musicVolume vào game khi Awake() để lát sau Start() tại PauseMenu có thể lấy đúng giá trị
            // **  - LƯU VOLUME HIỆN TẠI BẰNG PLAYERPREFS
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

        // ** 01. ENDHANCED MUSIC VOLUME SEGMENT
        // ** - LƯU VOLUME HIỆN TẠI BẰNG PLAYERPREFS
        // musicVolume = SaveSystem.LoadVolume();
        // ** END SEGMENT

        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;

            // ** 01. DEFAULT MUSIC SEGMENT  
            // ** - LƯU VOLUME HIỆN TẠI BẰNG PLAYERPREFS
            bgmSource.volume = musicVolume;
            // ** END SEGMENT

            // ** 01. ENHANCED MUSIC SEGMENT
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

            // ** 01. ENHANCED MUSIC SEGMENT
            // **  - LƯU VOLUME HIỆN TẠI BẰNG PLAYERPREFS
            // LƯU LẠI GIÁ TRỊ VÀO PLAYERPREFS
            // PlayerPrefs.SetFloat("Volume", Mathf.Max(0, musicVolume));
            // PlayerPrefs.Save();
            // ** END SEGMENT
        }
    }
    // ! LẤY GIÁ TRỊ VOLUME HIỆN TẠI
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    // !! 02. DỪNG NHẠC Làm cho nhạc dừng khi ra Menu chính?
    public void PauseMusic()
    {
        bgmSource.Pause();
    }
    // !! 02. TIẾP TỤC NHẠC
    public void ResumeMusic()
    {
        bgmSource.loop = true;
        bgmSource.volume = musicVolume;
        bgmSource.Play();
    }
}
