using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip music;
    }

    [Header("Audio Settings")]
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private List<SceneMusic> sceneMusicMappings = new List<SceneMusic>();
    [SerializeField] private float fadeDuration = 1f;

    private AudioClip targetClip;
    private Coroutine fadeCoroutine;

    //////////////////////////////////////////////////////////
    #region Initialization
    //////////////////////////////////////////////////////////

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Scene Music Handling
    //////////////////////////////////////////////////////////

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    private void PlaySceneMusic(string sceneName)
    {
        AudioClip newClip = GetMusicForScene(sceneName);

        if (newClip != null && newClip != mainAudioSource.clip)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeSwitchMusic(newClip));
        }
    }

    private AudioClip GetMusicForScene(string sceneName)
    {
        foreach (SceneMusic mapping in sceneMusicMappings)
        {
            if (mapping.sceneName == sceneName)
            {
                return mapping.music;
            }
        }
        return null;
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Fade Transitions
    //////////////////////////////////////////////////////////

    private IEnumerator FadeSwitchMusic(AudioClip newClip)
    {
        // Fade out
        yield return StartCoroutine(FadeVolume(0f, fadeDuration));

        // Switch clip
        mainAudioSource.Stop();
        mainAudioSource.clip = newClip;
        mainAudioSource.Play();

        // Fade in
        yield return StartCoroutine(FadeVolume(1f, fadeDuration));
    }

    private IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = mainAudioSource.volume;
        float time = 0;

        while (time < duration)
        {
            mainAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        mainAudioSource.volume = targetVolume;
    }

    #endregion

    //////////////////////////////////////////////////////////
    #region Cleanup
    //////////////////////////////////////////////////////////

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
}