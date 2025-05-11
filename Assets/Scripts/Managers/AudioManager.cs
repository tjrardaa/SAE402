using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Singleton accessible partout

    [Header("Audio Settings")]
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioMixerGroup soundEffectMixer;
    [SerializeField] private AudioMixerGroup musicEffectMixer;
    [SerializeField] private AudioClip[] playlist;

    [Header("Volume Control")]
    [SerializeField][Range(0f, 1f)] private float volumeOnPaused = 0.35f;
    [SerializeField][Range(0f, 1f)] private float volumeOnPlay = 1f;
    [SerializeField] private float volumeStep = 0.005f;

    [Header("Event Channels")]
    [SerializeField] private PlaySoundAtEventChannel sfxAudioChannel;

    private int musicIndex;

    //////////////////////////////////////////////////////////
    #region Initialization & Singleton
    //////////////////////////////////////////////////////////

    private void Awake()
    {
        ConfigureSingleton();
        InitializeAudioSource();
    }

    private void ConfigureSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Détruit les doublons
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Garde l'instance entre les scènes
    }

    private void InitializeAudioSource()
    {
        if (mainAudioSource == null)
        {
            mainAudioSource = GetComponent<AudioSource>();
            if (mainAudioSource == null)
            {
                Debug.LogError("Aucun AudioSource trouvé sur le GameObject !");
            }
        }
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Editor Validation
    //////////////////////////////////////////////////////////

    private void OnValidate()
    {
        // Avertissements dans l'éditeur si champs non assignés
        if (mainAudioSource == null) Debug.LogWarning("Assignez mainAudioSource !", this);
        if (playlist.Length == 0) Debug.LogWarning("Playlist vide !", this);
        if (sfxAudioChannel == null) Debug.LogWarning("Assignez sfxAudioChannel !", this);
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Music Management
    //////////////////////////////////////////////////////////

    private void Start()
    {
        if (playlist.Length > 0)
        {
            PlayMusic(0); // Démarre avec la première musique
        }
    }

    private void Update()
    {
        if (!mainAudioSource.isPlaying && playlist.Length > 0)
        {
            PlayNextMusic();
        }
    }

    private void PlayMusic(int index)
    {
        musicIndex = index;
        mainAudioSource.clip = playlist[musicIndex];
        mainAudioSource.outputAudioMixerGroup = musicEffectMixer;
        mainAudioSource.Play();
    }

    private void PlayNextMusic()
    {
        musicIndex = (musicIndex + 1) % playlist.Length;
        PlayMusic(musicIndex);
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Public Methods
    //////////////////////////////////////////////////////////

    public void PlayClipAt(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio: " + clip.name);
        tempGO.transform.position = position;
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = soundEffectMixer;
        audioSource.Play();
        Destroy(tempGO, clip.length);
    }

    public void OnTogglePause(bool isGamePaused)
    {
        StopAllCoroutines();
        StartCoroutine(AdjustVolume(isGamePaused ? volumeOnPaused : volumeOnPlay));
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Volume Fading
    //////////////////////////////////////////////////////////

    private IEnumerator AdjustVolume(float targetVolume)
    {
        float currentVolume = mainAudioSource.volume;
        float duration = Mathf.Abs(targetVolume - currentVolume) / volumeStep;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            mainAudioSource.volume = Mathf.Lerp(currentVolume, targetVolume, t / duration);
            yield return null;
        }

        mainAudioSource.volume = targetVolume;
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Event Subscriptions
    //////////////////////////////////////////////////////////

    private void OnEnable() => sfxAudioChannel.OnEventRaised += PlayClipAt;
    private void OnDisable() => sfxAudioChannel.OnEventRaised -= PlayClipAt;

    #endregion
}